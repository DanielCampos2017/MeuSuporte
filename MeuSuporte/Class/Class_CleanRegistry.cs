using Microsoft.Win32;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_CleanRegistry
    {
        private MainForm _MainForm;

        public Class_CleanRegistry(MainForm Form_)
        {
            _MainForm = Form_;
        }

        public async Task DeleteRegistry(CancellationToken token, int ValueUniProgressBar)
        {
            await Task.WhenAll(
                Reg_CURRENT_MACHAsync(token, ValueUniProgressBar / 3),
                Reg_CURRENT_USERAsync(token, ValueUniProgressBar / 3),
                Reg_CURRENT_Wow6432NodeAsync(token, ValueUniProgressBar / 3)
            );
        }

        private async Task Reg_CURRENT_MACHAsync(CancellationToken token, int ValueUniProgressBar)
        {
            // Usando o método OpenSubKey para acessar as chaves do registro
            using (RegistryKey Pasta_MACH_Run = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run"))
            using (RegistryKey Pasta_MACH_CurrentVersion = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (Pasta_MACH_Run?.ValueCount > 0)
                {
                    // Itera sobre todas as chaves dentro da pasta Run
                    foreach (string NomeChave in Pasta_MACH_Run.GetValueNames())
                    {
                        token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                        try
                        {
                            // Deleta a chave do registro
                            Pasta_MACH_CurrentVersion.DeleteValue(NomeChave);
                            await _MainForm.Log_MensagemAsync($"Registro: {NomeChave} Apagado !", true);
                            _MainForm.Sucesso++;
                        }
                        catch (Exception e)
                        {
                            _MainForm.Log_MensagemAsync($"Erro ao Apagar Registro: {NomeChave}", true);
                            _MainForm.Erro++;
                        }
                    }                    
                    _MainForm.ProgressBarADD(ValueUniProgressBar);
                }
                else
                {
                     await _MainForm.Log_MensagemAsync("Sem chave no Registro: MACHINE", true);
                }
            }
        }

        private async Task Reg_CURRENT_USERAsync(CancellationToken token, int ValueUniProgressBar)
        {
            using (RegistryKey Pasta_USER_Run = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run"))
            using (RegistryKey Pasta_USER_CurrentVersion = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (Pasta_USER_Run?.ValueCount > 0)
                {
                    // Itera sobre todas as chaves dentro da pasta Run
                    foreach (string NomeChave in Pasta_USER_Run.GetValueNames())
                    {
                        token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                        try
                        {
                            // Deleta a chave do registro
                            Pasta_USER_CurrentVersion.DeleteValue(NomeChave);                            
                            await _MainForm.Log_MensagemAsync($"Registro: {NomeChave} Apagado !", true);
                            _MainForm.Sucesso++;
                        }
                        catch (Exception e)
                        {
                             await _MainForm.Log_MensagemAsync($"Erro ao Apagar Registro: {NomeChave}", true);
                            _MainForm.Erro++;
                        }
                    }
                    _MainForm.ProgressBarADD(ValueUniProgressBar);              
                }
                else
                {
                    await _MainForm.Log_MensagemAsync("Sem chave no Registro: USER", true);
                }
            }
        }

        private async Task Reg_CURRENT_Wow6432NodeAsync(CancellationToken token, int ValueUniProgressBar)
        {
            // Usando o método OpenSubKey para acessar as chaves do registro
            using (RegistryKey Pasta_Node_Run = Registry.LocalMachine.OpenSubKey(@"Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Run"))
            using (RegistryKey Pasta_MACH_CurrentVersion = Registry.LocalMachine.OpenSubKey(@"Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (Pasta_Node_Run?.ValueCount > 0)
                {
                    // Itera sobre todas as chaves dentro da pasta Run
                    foreach (string NomeChave in Pasta_Node_Run.GetValueNames())
                    {
                        token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                        try
                        {
                            // Deleta a chave do registro
                            Pasta_MACH_CurrentVersion.DeleteValue(NomeChave);
                            await _MainForm.Log_MensagemAsync($"Registro: {NomeChave} Apagado !", true);
                            _MainForm.Sucesso++;
                        }
                        catch (Exception e)
                        {                            
                            Task task = _MainForm.Log_MensagemAsync($"Erro ao Apagar Registro: {NomeChave}", true);
                            _MainForm.Erro++;
                        }
                    }
                    _MainForm.ProgressBarADD(ValueUniProgressBar);                    
                }
                else
                {
                    _MainForm.Log_MensagemAsync("Sem chave no Registro: WOW6432Node", true);
                }
            }
        }

    }
}
