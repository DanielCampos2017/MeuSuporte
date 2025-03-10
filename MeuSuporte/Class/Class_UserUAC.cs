using Microsoft.Win32;
using MeuSuporte.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;

namespace TaskScheduler
{
    class Class_UserUAC
    {
        private MainForm _MainForm;

        public Class_UserUAC(MainForm formPreventiva)
        {
            _MainForm = formPreventiva;
        }

        public async Task NotificacaoUsuario(bool valor)
        {
            if (_MainForm.AbortExecution)
            {
                return;
            }
            _MainForm.PainelInfoDescricao(Resources.UserUAC, "Notificações ao Usuário (UAC):\n\rGerencia os alertas do Controle de Conta de Usuário (UAC), que ajudam a proteger o sistema contra alterações não autorizadas.");

            if(valor)
            {
                try
                {
                    using (RegistryKey pastaCurrentVersion = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies", true))
                    using (RegistryKey testSettings = pastaCurrentVersion.OpenSubKey("System", true))
                    {
                        testSettings.SetValue("ConsentPromptBehaviorAdmin", 2, RegistryValueKind.DWord); // Volta ao padrão (pedir confirmação)
                        testSettings.SetValue("PromptOnSecureDesktop", 1, RegistryValueKind.DWord); // Ativa a Área de Trabalho Segura
                        testSettings.SetValue("EnableLUA", 1, RegistryValueKind.DWord); // Ativa o UAC

                        _MainForm.Log_Mensagem("Notificações do Usuario UAC:", "Ativado");
                    }

                    // Força reinício do sistema para aplicar mudanças
                 //   Process.Start("shutdown", "/r /t 5");

                    _MainForm.Sucesso++;
                }
                catch (Exception)
                {
                    _MainForm.Log_Mensagem("Notificações do Usuario UAC:", "Erro!");
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

                        _MainForm.Log_Mensagem("Notificações do Usuario UAC:", "Desativado");
                    }

                    // Força reinício do sistema para aplicar mudanças
                  //  Process.Start("shutdown", "/r /t 5");

                    _MainForm.Sucesso++;
                }
                catch (Exception)
                {
                    _MainForm.Log_Mensagem("Notificações do Usuario UAC:", "Erro!");
                    _MainForm.Erro++;
                }
            }

        }


        void CodigoOLD()
        {

            if (true)
            {
                try
                {
                    using (RegistryKey pastaCurrentVersion = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies"))
                    using (RegistryKey testSettings = pastaCurrentVersion.OpenSubKey("System", true))
                    {
                        testSettings.SetValue("ConsentPromptBehaviorAdmin", 5, RegistryValueKind.DWord);
                        testSettings.SetValue("PromptOnSecureDesktop", 1, RegistryValueKind.DWord);
                        //   testSettings.SetValue("EnableLUA", 1, RegistryValueKind.DWord);

                        _MainForm.Log_Mensagem("Notificações do Usuario UAC:", "Desativado");
                    }
                    _MainForm.Sucesso++;
                }
                catch (Exception)
                {
                    _MainForm.Log_Mensagem("Notificações do Usuario UAC:", "Erro!");
                    _MainForm.Erro++;
                }
            }
            else
            {
                try
                {
                    using (RegistryKey pastaCurrentVersion = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies"))
                    using (RegistryKey testSettings = pastaCurrentVersion.OpenSubKey("System", true))
                    {
                        testSettings.SetValue("ConsentPromptBehaviorAdmin", 0, RegistryValueKind.DWord);
                        testSettings.SetValue("PromptOnSecureDesktop", 0, RegistryValueKind.DWord);
                        //    testSettings.SetValue("EnableLUA", 0, RegistryValueKind.DWord);

                        _MainForm.Log_Mensagem("Notificações do Usuario UAC:", "Ativado");
                    }
                    _MainForm.Sucesso++;
                }
                catch (Exception)
                {
                    _MainForm.Log_Mensagem("Notificações do Usuario UAC:", "Erro!");
                    _MainForm.Erro++;
                }
            }
        }



    }
}
