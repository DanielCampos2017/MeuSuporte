using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBackupBCD_Mananger
    {
        private  WinBackupBCD_ProcessController ProcessController;

        public async Task Mananger()
        {
            ProcessController = new WinBackupBCD_ProcessController();
            ProcessController.Create(WinGlobal_UIService.Instance.ValueUniProgressBar);
        }       
    }  
    
}
