using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBloatware_Mananger
    {      
        private WinBloatware_CheckInstallation CheckInstallation;

        public WinBloatware_Mananger()
        {
            CheckInstallation = new WinBloatware_CheckInstallation();
        }

        public async Task Mananger(CancellationToken token)
        {
            var Bloatware_List = new WinBloatware_List();
            List<WinBloatware_Format> Bloatware_Format = Bloatware_List.List(); // carrega a lista para struct

            foreach (var bloat in Bloatware_Format)
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                await CheckInstallation.Check(bloat, token); // Executa o a função Async em uma nova thread
            }            
        }

        
        //#removido com sucesso
        //Notícia {BingNews} - uninstall success

        //#falha na desinstalação
        //Notícia {BingNews} - uninstall failed

        //#não encontrado
        //Notícia {BingNews} - not found
    }
}
