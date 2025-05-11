using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinApp_Log
    {
        WinGlobal_DirectoryMananger DirectoryManange;
        public WinApp_Log()
        {
            DirectoryManange = new WinGlobal_DirectoryMananger();
        }

        public async Task GravaAsync(string dados)
        {
            string NameFolder = "Log";

            try
            {
                // Cria o diretorio
                if (DirectoryManange.Create(NameFolder) == false)
                {
                    return;
                }

                string FileName = $"log_{DateTime.Now:yyyyMMdd-HHmm}.ini";

                string caminhoArquivo = Path.Combine(DirectoryManange.GetDirectory(NameFolder), FileName);    //log_20250305-153045.txt

                // Abre o arquivo e adiciona a nova linha sem sobrescrever o conteúdo existente
                using (StreamWriter writer = new StreamWriter(caminhoArquivo, true, Encoding.Default))
                {
                    await writer.WriteLineAsync(dados);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao gravar log: {e.Message}");
            }
        }
    }
}
