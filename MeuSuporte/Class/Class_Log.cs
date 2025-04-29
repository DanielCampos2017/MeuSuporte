using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_Log
    {
        public async Task GravaAsync(string dados)
        {
            try
            {
                string diretorio = new Class_GeraNomePasta().Directory() + @"\Log";

                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                }

                string FileName = $"log_{DateTime.Now:yyyyMMdd-HHmm}.ini";

                string caminhoArquivo = Path.Combine(diretorio, FileName);    //log_20250305-153045.txt

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
