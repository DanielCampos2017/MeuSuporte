using System.IO;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinDirectory_Folder
    {
        private DirectoryInfo folder;
        private DirectorySecurity _DirectorySecurity = new DirectorySecurity();

        public async Task Delete(string txt, CancellationToken token) // deleta a pasta de forma assíncrona
        {
            folder = new System.IO.DirectoryInfo(txt); // atribui a pasta
            folder.SetAccessControl(_DirectorySecurity); // atribui o acesso para a pasta

            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                folder.Delete(true); // deleta a pasta
            }
            catch
            {
                // _formPreventiva.Log_Mensagem("Erro ao Apagar Pasta: ", txt);
            }
        }

    }
}
