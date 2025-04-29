using System;
using System.Threading.Tasks;
using MeuSuporte.Properties;
using Microsoft.Win32;

namespace MeuSuporte
{
    class Class_UserUAC
    {
        private MainForm _MainForm;

        public Class_UserUAC(MainForm Form_)
        {
            _MainForm = Form_;
        }

        public async Task NotificacaoUsuario(bool valor, int ValueUniProgressBar)
        {
            _MainForm.PainelInfoDescricao(Resources.UserUAC_Black, "Notificações ao Usuário (UAC):\n\rGerencia os alertas do Controle de Conta de Usuário (UAC), que ajudam a proteger o sistema contra alterações não autorizadas.");

             _MainForm.ProgressBarADD(ValueUniProgressBar / 2);

            if (valor)
            {
                try
                {
                    using (RegistryKey pastaCurrentVersion = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies", true))
                    using (RegistryKey testSettings = pastaCurrentVersion.OpenSubKey("System", true))
                    {
                        testSettings.SetValue("ConsentPromptBehaviorAdmin", 2, RegistryValueKind.DWord); // Volta ao padrão (pedir confirmação)
                        testSettings.SetValue("PromptOnSecureDesktop", 1, RegistryValueKind.DWord); // Ativa a Área de Trabalho Segura
                        testSettings.SetValue("EnableLUA", 1, RegistryValueKind.DWord); // Ativa o UAC

                        await _MainForm.Log_MensagemAsync("Notificações do Usuario UAC: Ativado", true);
                    }
                     _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
                    _MainForm.Sucesso++;
                }
                catch (Exception)
                {
                    _MainForm.Log_MensagemAsync("Notificações do Usuario UAC: Erro!", true);
                    _MainForm.Erro++;
                }
            }
            else
            {
                try
                {
                    using (RegistryKey pastaCurrentVersion = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies", true))
                    using (RegistryKey testSettings = pastaCurrentVersion.OpenSubKey("System", true))
                    {
                        testSettings.SetValue("ConsentPromptBehaviorAdmin", 0, RegistryValueKind.DWord);
                        testSettings.SetValue("PromptOnSecureDesktop", 0, RegistryValueKind.DWord);
                        testSettings.SetValue("EnableLUA", 0, RegistryValueKind.DWord); // Desativa o UAC

                        await _MainForm.Log_MensagemAsync("Notificações do Usuario UAC: Desativado", true);
                    }
                    _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
                    _MainForm.Sucesso++;
                }
                catch (Exception)
                {
                    _MainForm.Log_MensagemAsync("Notificações do Usuario UAC: Erro!", true);
                    _MainForm.Erro++;
                }
            }

        }

    }
}
