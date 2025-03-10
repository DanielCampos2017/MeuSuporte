using Microsoft.Win32;
using System;
using System.Threading.Tasks;

namespace TaskScheduler
{
    internal class Class_CleanRegistry
    {
        private MainForm _MainForm;

        public Class_CleanRegistry(MainForm formPreventiva)
        {
            _MainForm = formPreventiva;
        }

        public async Task DeleteRegistry()
        {
            await Task.WhenAll(
                Reg_CURRENT_MACHAsync(),
                Reg_CURRENT_USERAsync()
            );
        }

        private async Task Reg_CURRENT_MACHAsync()
        {
            // Usando o método OpenSubKey para acessar as chaves do registro
            using (RegistryKey Pasta_MACH_Run = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run"))
            using (RegistryKey Pasta_MACH_CurrentVersion = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion", true))
            {
                if (Pasta_MACH_Run?.ValueCount > 0)
                {
                    // Itera sobre todas as chaves dentro da pasta Run
                    foreach (string NomeChave in Pasta_MACH_Run.GetValueNames())
                    {
                        try
                        {
                            // Deleta a chave do registro
                            Pasta_MACH_CurrentVersion.DeleteValue(NomeChave);
                             _MainForm.Log_Mensagem("Registro: ", NomeChave + " Apagado !");
                        }
                        catch (Exception e)
                        {
                            _MainForm.Erro++;
                            _MainForm.Log_Mensagem("Erro ao Apagar Registro: ", NomeChave);
                        }
                    }
                    _MainForm.Sucesso++;
                }
                else
                {
                     _MainForm.Log_Mensagem("Sem chave no Registro:", "MACHINE");
                }
            }
        }

        private async Task Reg_CURRENT_USERAsync()
        {
            if (_MainForm.AbortExecution)
            {
                return;
            }

            using (RegistryKey Pasta_USER_Run = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run"))
            using (RegistryKey Pasta_USER_CurrentVersion = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion", true))
            {
                if (Pasta_USER_Run?.ValueCount > 0)
                {
                    // Itera sobre todas as chaves dentro da pasta Run
                    foreach (string NomeChave in Pasta_USER_Run.GetValueNames())
                    {
                        try
                        {
                            // Deleta a chave do registro
                            Pasta_USER_CurrentVersion.DeleteValue(NomeChave);
                            _MainForm.Erro++;
                            _MainForm.Log_Mensagem("Registro: ", NomeChave + " Apagado !");
                        }
                        catch (Exception e)
                        {
                             _MainForm.Log_Mensagem("Erro ao Apagar Registro: ", NomeChave);
                        }
                    }
                    _MainForm.Sucesso++;
                }
                else
                {
                    _MainForm.Log_Mensagem("Sem chave no Registro:", "USER");
                }
            }
        }
    }
}
