using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinPageFile_Mananger
    {
        private WinPageFile_KeyRegistry KeyRegistry;

        public WinPageFile_Mananger()
        {
            KeyRegistry = new WinPageFile_KeyRegistry();
        }
                
        public async Task Mananger(bool valor, int ValueUniProgressBar, CancellationToken token)
        {
            KeyRegistry.State(valor, ValueUniProgressBar, token);       
        }
    }
}
