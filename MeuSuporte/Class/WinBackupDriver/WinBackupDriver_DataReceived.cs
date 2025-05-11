using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBackupDriver_DataReceived
    {
        private readonly WinGlobal_UIService UIService;
        WinBackupDriver_ExtractData WinBackupDriver_ExtractProgress;

        public WinBackupDriver_DataReceived(WinGlobal_UIService ui)
        {
            UIService = ui;
            WinBackupDriver_ExtractProgress = new WinBackupDriver_ExtractData(UIService);
        }

        //Trata a saida do Processo
        public async Task Received(Process process)
        {
            process.OutputDataReceived += async (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    await WinBackupDriver_ExtractProgress.DriverBackup_ExtractProgress(e.Data);
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    UIService.Erro++;
                    Task task = UIService.Log_MensagemAsync("Erro DISM: " + e.Data, true);
                }
            };
        }

    }
}
