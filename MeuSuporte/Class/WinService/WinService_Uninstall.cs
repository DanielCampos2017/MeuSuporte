using System;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinService_Uninstall
    {
        private readonly WinService_Stop _serviceStopper;

        public WinService_Uninstall()
        {
            _serviceStopper = new WinService_Stop(); // criada apenas uma vez
        }

        public async Task<bool> TryUninstallAsync(ServiceController service)
        {

            bool isServiceStopped = await _serviceStopper.WaitForServiceToStop(service);

            if (!isServiceStopped)
            {
                await WinGlobal_UIService.Instance.Log_MensagemAsync($"Serviço: {service.DisplayName} não pode ser removido pois ainda está em execução.", true);
                return false;
            }

            try
            {
                using (var serviceInstaller = new ServiceInstaller())
                {
                    serviceInstaller.Context = new InstallContext(null, null);
                    serviceInstaller.ServiceName = service.ServiceName;
                    serviceInstaller.Uninstall(null);

                    await WinGlobal_UIService.Instance.Log_MensagemAsync($"Serviço: {service.DisplayName} - Deleted", true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                WinGlobal_UIService.Instance.Erro++;
                await WinGlobal_UIService.Instance.Log_MensagemAsync($"Erro ao desinstalar o Serviço: {service.DisplayName}\n{ex.Message}", true);
                return false;
            }
        }
    }
}
