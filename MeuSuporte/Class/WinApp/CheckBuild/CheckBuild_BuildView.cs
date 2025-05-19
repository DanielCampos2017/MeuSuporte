using System.Reflection;
using System;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class CheckBuild_BuildView
    {
        private readonly CheckBuild_GetGitHub GetGitHub;
        private readonly CheckBuild_Process Build_Process;
      
        public CheckBuild_BuildView()
        {
            GetGitHub = new CheckBuild_GetGitHub();
            Build_Process = new CheckBuild_Process();
        }

        public async Task VersionBuild(string GitHubRepo)
        {
            // Usando Invoke para atualizar a interface do usuário a partir da thread de segundo plano
            WinGlobal_UIService.Instance.InterfaceGUI.Invoke(new Action(() =>
            {
                WinGlobal_UIService.Instance.InterfaceGUI.Text = $"MeuSuporte Build Checking...";
            }));

            string BuildLocal = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0"; // Obtem a versão local
            string Buildgithub = await GetGitHub.GetLatestVersion(GitHubRepo); // Obtem a versão do GitHub

            //Converte o valores
            Version VersionLocal = new Version(BuildLocal);
            Version VersionGitHub = new Version(Buildgithub);
            //Compara os valores
            int result = VersionLocal.CompareTo(VersionGitHub);

            // exibe as mensagem do resultado
            Build_Process.Mensage(VersionGitHub, result, GitHubRepo);            
        }
    }
}
