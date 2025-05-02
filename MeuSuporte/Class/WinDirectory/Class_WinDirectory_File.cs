using System.IO;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_WinDirectory_File
    {
        private FileSecurity _FileSecurity = new FileSecurity();
        private FileInfo file;

        public async Task Delete(string txt, CancellationToken token) // deleta o arquivo de forma assíncrona
        {
            file = new FileInfo(txt); // atribui o arquivo
            file.SetAccessControl(_FileSecurity); // atribui o acesso

            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                file.Delete(); // deleta o arquivo               
            }
            catch
            {
                // não vou querer mensagem de erro devido ser centenas de arquivos para apagar então não quero entopir o log com erro
            }
        }
    }
}
