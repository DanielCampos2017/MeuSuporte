using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace MeuSuporte
{
    internal class CheckBuild_GetVersion
    {

        public async Task<string> GetLatestVersionFromGitHub(string repo)
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
