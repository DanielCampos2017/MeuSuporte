using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MeuSuporte
{
    internal class WinPageFile_KeyRegistry
    {
        private readonly WinGlobal_UIService UIService;
        public WinPageFile_KeyRegistry(WinGlobal_UIService ui)
        {
            UIService = ui;
        }

        public async Task State(bool state , int ValueUniProgressBar, CancellationToken token)
        {
            try
            {
                int value = Convert.ToInt32(state);

                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                UIService.ProgressBarADD(ValueUniProgressBar / 2);

                Microsoft.Win32.RegistryKey PastaCurrentVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager");

                using (RegistryKey testSettings = PastaCurrentVersion.OpenSubKey("Memory Management", true))
                {
                    testSettings.SetValue("ClearPageFileAtShutdown", Convert.ToInt32(state), RegistryValueKind.DWord);
                }

                UIService.Sucesso++;
                await UIService.Log_MensagemAsync("PageFile.sys:  Limpesa altomatico: Ativado !", true);
                await Task.Delay(500);
                UIService.ProgressBarADD(ValueUniProgressBar / 2);
            }
            catch (Exception e)
            {
                UIService.Erro++;
                await UIService.Log_MensagemAsync($"PageFile.sys:  Erro ! {e.Message}", true);

            }
        }
    }
}
