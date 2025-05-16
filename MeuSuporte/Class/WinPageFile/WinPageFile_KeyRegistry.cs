using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MeuSuporte
{
    internal class WinPageFile_KeyRegistry
    {
        public async Task State(bool state , int ValueUniProgressBar, CancellationToken token)
        {
            try
            {
                int value = Convert.ToInt32(state);

                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                WinGlobal_UIService2.Instance.ProgressBarADD(ValueUniProgressBar / 2);

                Microsoft.Win32.RegistryKey PastaCurrentVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager");

                using (RegistryKey testSettings = PastaCurrentVersion.OpenSubKey("Memory Management", true))
                {
                    testSettings.SetValue("ClearPageFileAtShutdown", Convert.ToInt32(state), RegistryValueKind.DWord);
                }

                WinGlobal_UIService2.Instance.Sucesso++;
                await WinGlobal_UIService2.Instance.Log_MensagemAsync("PageFile.sys:  Limpesa altomatico: Ativado !", true);
                await Task.Delay(500);
                WinGlobal_UIService2.Instance.ProgressBarADD(ValueUniProgressBar / 2);
            }
            catch (Exception e)
            {
                WinGlobal_UIService2.Instance.Erro++;
                await WinGlobal_UIService2.Instance.Log_MensagemAsync($"PageFile.sys:  Erro ! {e.Message}", true);
            }
        }
    }
}
