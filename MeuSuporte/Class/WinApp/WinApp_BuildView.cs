using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MeuSuporte.Properties;

namespace MeuSuporte
{
    class WinApp_BuildView
    {
        private readonly WinGlobal_UIService UIService;

        string GitHubRepo = "DanielCampos2017/MeuSuporte";

        public WinApp_BuildView(WinGlobal_UIService ui)
        {
            UIService = ui;
        }

        public void Build()
        {
            Thread thread = new Thread(CheckBuild);
            thread.IsBackground = true; 
            thread.Start();
        }

        // Função principal que será executada na thread
        private void CheckBuild()
        {
            string BuildLocal = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0";
            string Buildgithub = "0.0.0.0";

            // Usando Invoke para atualizar a interface do usuário a partir da thread de segundo plano
            UIService.InterfaceGUI.Invoke(new Action(() =>
            {
                UIService.InterfaceGUI.Text = $"MeuSuporte Build {BuildLocal} - checking";
            }));

            // Verifica a conectividade com a internet
            if (IsInternetAvailable())
            {
                Buildgithub = GetLatestVersionFromGitHub(GitHubRepo).Result;
            }
            else
            {
                // Usando Invoke para atualizar a interface do usuário a partir da thread de segundo plano
                UIService.InterfaceGUI.Invoke(new Action(() =>
                {
                    UIService.InterfaceGUI.Text = $"MeuSuporte Build {BuildLocal} - No Connection";
                    UIService.InterfaceGUI.PainelInfoDescricao(Resources.Offline_Black, $"Sem Conexão:\r\nNão foi possível conectar ao repositório GitHub. https://github.com/{GitHubRepo}/releases/latest");
                }));
                return; // Retorna aqui para evitar continuar o processo sem a versão do GitHub
            }

            Version vLocal = new Version(BuildLocal);
            Version vLatest = new Version(Buildgithub);

            int result = vLocal.CompareTo(vLatest);

            if (vLatest == new Version(0, 0, 0, 0))
            {
                // Usando Invoke para atualizar a interface do usuário a partir da thread de segundo plano
                UIService.InterfaceGUI.Invoke(new Action(() =>
                {
                    UIService.InterfaceGUI.Text = $"MeuSuporte Build {BuildLocal} - Unknown";  // Versão não encontrada no GitHub
                    UIService.InterfaceGUI.PainelInfoDescricao(Resources.Alert_Black, $"404 Not Found!:\r\nNão foi possível encontrar repositório. https://github.com/{GitHubRepo}/releases/latest");
                }));
            }
            else
            {
                if (result > 0)
                {
                    // Usando Invoke para atualizar a interface do usuário a partir da thread de segundo plano
                    UIService.InterfaceGUI.Invoke(new Action(() =>
                    {
                        UIService.InterfaceGUI.Text = $"MeuSuporte Build {BuildLocal} - Beta"; // Versão local maior que a do GitHub
                        UIService.InterfaceGUI.PainelInfoDescricao(Resources.Code_Black, $"Verão Beta\r\nEssa é uma versão de desenvolvimento. Que ainda se encontra em desenvolvimento e não trata de uma versão final !");
                    }));
                }
                else if (result < 0)
                {
                    // Usando Invoke para atualizar a interface do usuário a partir da thread de segundo plano
                    UIService.InterfaceGUI.Invoke(new Action(() =>
                    {
                        UIService.InterfaceGUI.checkBoxAllState(false);
                        UIService.InterfaceGUI.Text = $"MeuSuporte Build {BuildLocal} - Legacy"; // Versão local inferior à do GitHub                       
                        UIService.InterfaceGUI.PainelInfoDescricao(Resources.Dawnload_Black, $"Atualização Disponível!:\r\nPara garantir o melhor eficiência baixe últimas versão. https://github.com/{GitHubRepo}/releases/latest");
                    }));
                }
                else
                {
                    // Usando Invoke para atualizar a interface do usuário a partir da thread de segundo plano
                    UIService.InterfaceGUI.Invoke(new Action(() =>
                    {
                        UIService.InterfaceGUI.Text = $"MeuSuporte Build {BuildLocal} - Stable"; // Versão local igual à do GitHub
                        UIService.InterfaceGUI.PainelInfoDescricao(Resources.Security_Black, $"Estável:\r\nSua verão se encontrar na mesma versão do repositório. https://github.com/{GitHubRepo}/releases/latest");
                    }));
                }
            }
        }

        // Verifica se a internet está disponível
        private bool IsInternetAvailable()
        {
            try
            {
                using (var ping = new Ping())
                {
                    // Ping para o servidor Google, você pode usar outro servidor de sua escolha
                    var reply = ping.Send("8.8.8.8", 3000); // Timeout de 3 segundos
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false; // Em caso de erro, não há conexão com a internet
            }
        }
         
        static async Task<string> GetLatestVersionFromGitHub(string repo)
        {
            using (HttpClient client = new HttpClient()) // Compatível com C# 7.3
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

                try
                {
                    string url = $"https://api.github.com/repos/{repo}/releases/latest";
                    string json = await client.GetStringAsync(url);

                    // Encontrando a posição do "tag_name" no JSON
                    string tagNameKeyword = "\"tag_name\":\"";
                    int startIndex = json.IndexOf(tagNameKeyword);

                    if (startIndex == -1)
                    {
                        return "0.0.0.0"; // Se não encontrar "tag_name", retorna um valor padrão
                    }

                    startIndex += tagNameKeyword.Length;
                    int endIndex = json.IndexOf("\"", startIndex);

                    if (endIndex == -1)
                    {
                        return "0.0.0.0"; // Se não encontrar o final da string, retorna um valor padrão
                    }

                    // Extrair o valor de "tag_name"
                    string tagName = json.Substring(startIndex, endIndex - startIndex);
                    return tagName ?? "0.0.0.0";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao obter a versão do GitHub: {ex.Message}");
                    return "0.0.0.0";
                }
            }
        }
    }
}
