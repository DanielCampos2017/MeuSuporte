using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBackupBCD_Mananger
    {
        private WinBackupBCD_ProcessController ProcessController;

        public WinBackupBCD_Mananger()
        {
            ProcessController = new WinBackupBCD_ProcessController();
        }

        public async Task Mananger(CancellationToken token, int ValueUniProgressBar)
        {
            ProcessController.Create( token, ValueUniProgressBar);
        }       
    }  
    
}
