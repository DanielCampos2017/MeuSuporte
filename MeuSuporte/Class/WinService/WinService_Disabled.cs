using System.ServiceProcess;
using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MeuSuporte
{
    internal class WinService_Disabled
    {

        private readonly WinService_Stop _serviceStopper;

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool ChangeServiceConfig(
        IntPtr hService, uint nServiceType, uint nStartType,
        uint nErrorControl, string lpBinaryPathName, string lpLoadOrderGroup,
        IntPtr lpdwTagId, [In] char[] lpDependencies, string lpServiceStartName,
        string lpPassword, string lpDisplayName);

        private const uint SERVICE_NO_CHANGE = 0xFFFFFFFF;

        public WinService_Disabled()
        {
            _serviceStopper = new WinService_Stop(); // criada apenas uma vez
        }

        public async Task WaitForServiceToDisabled(ServiceController service)
        {
            bool isServiceStopped = await _serviceStopper.WaitForServiceToStop(service); // Para o serviço   

            if (!isServiceStopped)
            {
                WinGlobal_UIService.Instance.Log_MensagemAsync($"Serviço: {service.DisplayName} não pode ser desabilitado devido ainda está em execução ", true);
                await Task.Delay(500);
                WinGlobal_UIService.Instance.Erro++;
                return;
            }

            if (service.StartType != ServiceStartMode.Disabled)
            {
                try
                {
                    using (var serviceHandle = service.ServiceHandle)
                    {
                        ChangeServiceConfig(
                            serviceHandle.DangerousGetHandle(),
                            SERVICE_NO_CHANGE, (uint)ServiceStartMode.Disabled,
                            SERVICE_NO_CHANGE, null, null,
                            IntPtr.Zero, null, null, null, null
                            );
                    }

                    WinGlobal_UIService.Instance.Log_MensagemAsync($"Serviço:  {service.DisplayName} - Disabled", true);
                    await Task.Delay(500);
                    WinGlobal_UIService.Instance.Sucesso++;
                }
                catch (Exception ex)
                {
                    WinGlobal_UIService.Instance.Log_MensagemAsync($"Erro ao Tentar mudar o Modo de Inicialização do Serviço: {service.DisplayName} " + ex.Message, true);
                    await Task.Delay(500);
                    WinGlobal_UIService.Instance.Erro++;
                }
            }

        }
    }
}
