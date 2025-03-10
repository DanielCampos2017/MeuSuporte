using Microsoft.Win32;
using System;
using System.Threading.Tasks;

namespace TaskScheduler
{
    internal class Class_CleanPageFile
    {
        private MainForm _MainForm;

        public Class_CleanPageFile(MainForm formPreventiva)
        {
            _MainForm = formPreventiva;
        }

        private async Task DesativadoAsync()
        {
            if (_MainForm.AbortExecution)
            {
                return;
            }
            try
            {
                Microsoft.Win32.RegistryKey PastaCurrentVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager");

                using (RegistryKey testSettings = PastaCurrentVersion.OpenSubKey("Memory Management", true))
                {
                    testSettings.SetValue("ClearPageFileAtShutdown", 0, RegistryValueKind.DWord);
                    _MainForm.Sucesso++;
                    _MainForm.Log_Mensagem("PageFile.sys:", " Limpesa altomatico: Desativado !");
                }
            }
            catch (Exception e)
            {
                _MainForm.Erro++;
                _MainForm.Log_Mensagem("PageFile.sys:", " Erro !");
            }
        }

        private async Task AtivadoAsync()
        {
            try
            {
                Microsoft.Win32.RegistryKey PastaCurrentVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager");

                using (RegistryKey testSettings = PastaCurrentVersion.OpenSubKey("Memory Management", true))
                {
                    testSettings.SetValue("ClearPageFileAtShutdown", 1, RegistryValueKind.DWord);
                    _MainForm.Sucesso++;
                    _MainForm.Log_Mensagem("PageFile.sys:", " Limpesa altomatico: Ativado !");
                }
            }
            catch (Exception e)
            {
                _MainForm.Erro++;
                _MainForm.Log_Mensagem("PageFile.sys:", " Erro !");
            }
        }

        public async Task CleanPageFile(bool valor)
        {
            if (valor)
            {
                await Task.Run(() => AtivadoAsync());
            }
            else
            {
                await Task.Run(() => DesativadoAsync());
            }
        }
    }
}
