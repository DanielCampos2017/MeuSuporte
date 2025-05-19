using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MeuSuporte
{
    internal class WinRegistryBin_WOW6432Node
    {
        public async Task Delete(int ValueUniProgressBar)
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
                        WinGlobal_UIService.Instance.token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                        try
                        {
                            // Deleta a chave do registro
                            Pasta_MACH_CurrentVersion.DeleteValue(NomeChave);
                            await WinGlobal_UIService.Instance.Log_MensagemAsync($"Registro: {NomeChave} Apagado !", true);
                            WinGlobal_UIService.Instance.Sucesso++;
                        }
                        catch (Exception e)
                        {
                            Task task = WinGlobal_UIService.Instance.Log_MensagemAsync($"Erro ao Apagar Registro: {NomeChave}", true);
                            WinGlobal_UIService.Instance.Erro++;
                        }
                    }
                    WinGlobal_UIService.Instance.ProgressBarADD(ValueUniProgressBar);
                }
                else
                {
                    WinGlobal_UIService.Instance.Log_MensagemAsync("Sem chave no Registro: WOW6432Node", true);
                }
            }
        }
    }
}
