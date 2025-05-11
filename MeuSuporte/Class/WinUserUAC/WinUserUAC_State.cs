using System;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MeuSuporte
{
    internal class WinUserUAC_State
    {
        private readonly WinGlobal_UIService UIService;
        public WinUserUAC_State(WinGlobal_UIService ui)
        {
            UIService = ui;
        }

        public async Task Notification(bool State, int ValueUniProgressBar)
        {
            int consentPrompt = State ? 2 : 0;
            int promptOnSecureDesktop = State ? 1 : 0;
            int enableLUA = State ? 1 : 0;

            UIService.ProgressBarADD(ValueUniProgressBar / 2);
            try
            {
                using (RegistryKey pastaCurrentVersion = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies", writable: true))
                using (RegistryKey testSettings = pastaCurrentVersion.OpenSubKey("System", writable: true))
                {
                    testSettings.SetValue("ConsentPromptBehaviorAdmin", consentPrompt, RegistryValueKind.DWord); // Volta ao padrão (pedir confirmação) = Ativado(2) Desativado(0)
                    testSettings.SetValue("PromptOnSecureDesktop", promptOnSecureDesktop, RegistryValueKind.DWord); // Ativa a Área de Trabalho Segura = Ativado(1) Desativado(0)
                    testSettings.SetValue("EnableLUA", enableLUA, RegistryValueKind.DWord); // Ativa o UAC = Ativado(1) Desativado(0)

                    await UIService.Log_MensagemAsync("Notificações do Usuario UAC: Alterado", true);                 
                }
                UIService.ProgressBarADD(ValueUniProgressBar / 2);
                UIService.Sucesso++;
            }
            catch (Exception)
            {
                UIService.Log_MensagemAsync("Notificações do Usuario UAC: Erro!", true);
                UIService.Erro++;
            }
        }
    }
}
