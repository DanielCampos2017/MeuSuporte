using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBackupDriver_DataReceived
    {
        private WinBackupDriver_ExtractData WinBackupDriver_ExtractProgress;

        public WinBackupDriver_DataReceived()
        {
            WinBackupDriver_ExtractProgress = new WinBackupDriver_ExtractData();
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
                    WinGlobal_UIService2.Instance.Erro++;
                    Task task = WinGlobal_UIService2.Instance.Log_MensagemAsync("Erro DISM: " + e.Data, true);
                }
            };
        }

    }
}
