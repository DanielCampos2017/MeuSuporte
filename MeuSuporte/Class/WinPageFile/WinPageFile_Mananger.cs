using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinPageFile_Mananger
    {
        WinPageFile_KeyRegistry KeyRegistry;
        public WinPageFile_Mananger(WinGlobal_UIService ui)
        {
            KeyRegistry = new WinPageFile_KeyRegistry(ui);
        }
                
        public async Task Mananger(bool valor, int ValueUniProgressBar, CancellationToken token)
        {
            KeyRegistry.State(valor, ValueUniProgressBar, token);
       
        }
    }
}
