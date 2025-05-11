using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MeuSuporte
{
    internal class WinRemoteRDP_Enable
    {
        private readonly WinGlobal_UIService UIService;
        private WinRemoteRDP_Registry RemoteRDP_Registry;
        private WinRemoteRDP_Service RemoteRDP_Service;
        private WinRemoteRDP_Firewall RemoteRDP_Firewall;
        int valor = 0;
        public WinRemoteRDP_Enable(WinGlobal_UIService ui)
        {
            UIService = ui;
            RemoteRDP_Registry = new WinRemoteRDP_Registry();
            RemoteRDP_Service = new WinRemoteRDP_Service();
            RemoteRDP_Firewall = new WinRemoteRDP_Firewall(UIService, valor);
        }

        public async Task Enable(CancellationToken token, int ValueUniProgressBar)
        {
            valor = ValueUniProgressBar / 3;

            try
            {
                UIService.ProgressBarADD(ValueUniProgressBar / 3);
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                //Habilita registro 
                await RemoteRDP_Registry.Enable();

                await UIService.Log_MensagemAsync($"Acesso Remoto Ativado", true);

                await Task.Delay(200);

                UIService.ProgressBarADD(ValueUniProgressBar / 3);

                //Inicia o Serviço TermService
                RemoteRDP_Service.ServiceEnable();

                // Habilita as regras do Firewall para RDP (porta 3389)
                RemoteRDP_Firewall.Rule();
                
            }
            catch (Exception ex)
            {
                await UIService.Log_MensagemAsync($"Erro: " + ex.Message, true);
            }
        }
    }
}
