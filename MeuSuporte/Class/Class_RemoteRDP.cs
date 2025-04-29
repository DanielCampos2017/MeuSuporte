using System;
using System.Diagnostics;
using System.Security.Principal;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MeuSuporte.Class
{
    internal class Class_RemoteRDP
    {
        private MainForm _MainForm;

        public Class_RemoteRDP(MainForm Form_)
        {
            _MainForm = Form_;
        }

        private async Task Enable(CancellationToken token, int ValueUniProgressBar)
        {
            try
            {
                _MainForm.ProgressBarADD(ValueUniProgressBar / 3);
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                //Habilita registro 
                using (RegistryKey chave = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server", true))
                {
                    if (chave != null)
                    {
                        chave.SetValue("fDenyTSConnections", 0, RegistryValueKind.DWord); // 0 = habilita
                    }
                }

                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", true))
                {
                    if (key != null)
                        key.SetValue("UserAuthentication", 0, RegistryValueKind.DWord);
                }

                await _MainForm.Log_MensagemAsync($"Acesso Remoto Ativado", true);

                await Task.Delay(200);

                _MainForm.ProgressBarADD(ValueUniProgressBar / 3);
                //Inicia o Serviço TermService
                using (ServiceController sc = new ServiceController("TermService"))
                {
                    if (sc.Status != ServiceControllerStatus.Running)
                    {
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                        await Task.Delay(500);
                    }
                }

                // Habilita as regras do Firewall para RDP (porta 3389)
                await Task.Delay(200);
                _MainForm.ProgressBarADD(ValueUniProgressBar / 3);
                ProcessStartInfo psi = new ProcessStartInfo("netsh", $"advfirewall firewall add rule name=\"Remote Desktop - TCP\" dir=in action=allow protocol=TCP localport=3389 profile=Domain,Private,Public")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    Verb = "runas"
                };

                using (Process processo = Process.Start(psi))
                {
                    processo.WaitForExit();
                    string saida = processo.StandardOutput.ReadToEnd();

                    if (processo.ExitCode == 0)
                    {
                        await _MainForm.Log_MensagemAsync($"Regra [Remote Desktop - TCP] adicionada com sucesso.", true);
                    }
                    else
                    {
                        await _MainForm.Log_MensagemAsync($"Erro ao tentar adicionar a regra [Remote Desktop - TCP] no firewall.", true);
                    }                
                }

            }
            catch (Exception ex)
            {
                await _MainForm.Log_MensagemAsync($"Erro: " + ex.Message, true);
            }
        }

        private async Task Disable(CancellationToken token, int ValueUniProgressBar)
        {
            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                
                _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
                // Habilita o Remote Desktop no registro
                using (RegistryKey chave = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server", true))
                {
                    if (chave != null)
                    {
                        chave.SetValue("fDenyTSConnections", 1, RegistryValueKind.DWord); // 1 = Desabilita
                    }
                }
                await _MainForm.Log_MensagemAsync($"Acesso Remoto Desativado", true);
                _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
            }
            catch (Exception ex)
            {
                await _MainForm.Log_MensagemAsync($"Erro: " + ex.Message, true);
            }
        }

        public async Task ConnectionRDP(bool valor, CancellationToken token, int ValueUniProgressBar)
        {
            if (valor)
            {
                await  Enable(token, ValueUniProgressBar);
            }
            else
            {
                await Disable(token, ValueUniProgressBar);
            }
        }

    }
}
