using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinRemoteRDP_Firewall
    {
        private readonly WinGlobal_UIService UIService;
        int ValueUniProgressBar = 0;
        public WinRemoteRDP_Firewall(WinGlobal_UIService ui, int Value)
        {
            UIService = ui;
            ValueUniProgressBar = Value;
        }


        public async Task Rule()
        {
            await Task.Delay(200);
            UIService.ProgressBarADD(ValueUniProgressBar);
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
                    await UIService.Log_MensagemAsync($"Regra [Remote Desktop - TCP] adicionada com sucesso.", true);
                }
                else
                {
                    await UIService.Log_MensagemAsync($"Erro ao tentar adicionar a regra [Remote Desktop - TCP] no firewall.", true);
                }
            }
        }
    }
}
