using Microsoft.Win32;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_CleanPageFile
    {
        private MainForm _MainForm;

        public Class_CleanPageFile(MainForm Form_)
        {
            _MainForm = Form_;
        }

        private async Task DesativadoAsync(CancellationToken token, int ValueUniProgressBar)
        {           
            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                _MainForm.ProgressBarADD(ValueUniProgressBar / 2);

                Microsoft.Win32.RegistryKey PastaCurrentVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager");

                using (RegistryKey testSettings = PastaCurrentVersion.OpenSubKey("Memory Management", true))
                {
                    testSettings.SetValue("ClearPageFileAtShutdown", 0, RegistryValueKind.DWord);
                }

                _MainForm.Sucesso++;
                await _MainForm.Log_MensagemAsync("PageFile.sys:  Limpesa altomatico: Desativado !", true);
                await Task.Delay(500);
                _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
            }
            catch (Exception e)
            {
                _MainForm.Erro++;
                await _MainForm.Log_MensagemAsync($"PageFile.sys: Erro {e.Message}", true);
                await Task.Delay(500);
            }
        }

        private async Task AtivadoAsync(CancellationToken token, int ValueUniProgressBar)
        {
            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                _MainForm.ProgressBarADD(ValueUniProgressBar / 2);

                Microsoft.Win32.RegistryKey PastaCurrentVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager");

                using (RegistryKey testSettings = PastaCurrentVersion.OpenSubKey("Memory Management", true))
                {
                    testSettings.SetValue("ClearPageFileAtShutdown", 1, RegistryValueKind.DWord);
                }

                _MainForm.Sucesso++;
                await _MainForm.Log_MensagemAsync("PageFile.sys:  Limpesa altomatico: Ativado !", true);
                await Task.Delay(500);
                _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
            }
            catch (Exception e)
            {
                _MainForm.Erro++;
                await _MainForm.Log_MensagemAsync($"PageFile.sys:  Erro ! {e.Message}", true);

            }
        }

        public async Task CleanPageFile(bool valor, CancellationToken token, int ValueUniProgressBar)
        {
            if (valor)
            {
                await  AtivadoAsync(token, ValueUniProgressBar);
            }
            else
            {
                await DesativadoAsync(token, ValueUniProgressBar);
            }
        }
    }
}
