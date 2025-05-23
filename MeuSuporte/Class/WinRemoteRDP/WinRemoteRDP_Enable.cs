﻿using System;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinRemoteRDP_Enable
    {
        private  WinRemoteRDP_Registry RemoteRDP_Registry;
        private  WinRemoteRDP_Service RemoteRDP_Service;
        private  WinRemoteRDP_Firewall RemoteRDP_Firewall;
    
        public async Task Enable()
        {
            RemoteRDP_Registry = new WinRemoteRDP_Registry();
            RemoteRDP_Service = new WinRemoteRDP_Service();
            RemoteRDP_Firewall = new WinRemoteRDP_Firewall();

            try
            {             
                WinGlobal_UIService.Instance.token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                //Habilita registro 
                await RemoteRDP_Registry.Enable();
                await WinGlobal_UIService.Instance.Log_MensagemAsync($"Acesso Remoto Ativado", true);
                await Task.Delay(200);
                                
                RemoteRDP_Service.ServiceEnable(); //Inicia o Serviço TermService
                RemoteRDP_Firewall.Rule(); // Habilita as regras do Firewall para RDP (porta 3389)

            }
            catch (Exception ex)
            {
                await WinGlobal_UIService.Instance.Log_MensagemAsync($"Erro: " + ex.Message, true);
            }
        }
    }
}
