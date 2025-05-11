using System;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinService_Uninstall
    {
        private readonly WinGlobal_UIService UIService;
        private readonly WinService_Stop _serviceStopper;

        public WinService_Uninstall(WinGlobal_UIService ui)
        {
            UIService = ui;
            _serviceStopper = new WinService_Stop(UIService); // criada apenas uma vez
        }

        public async Task<bool> TryUninstallAsync(ServiceController service)
        {

            bool isServiceStopped = await _serviceStopper.WaitForServiceToStop(service);

            if (!isServiceStopped)
            {
                await UIService.Log_MensagemAsync($"Serviço: {service.DisplayName} não pode ser removido pois ainda está em execução.", true);
                return false;
            }

            try
            {
                using (var serviceInstaller = new ServiceInstaller())
                {
                    serviceInstaller.Context = new InstallContext(null, null);
                    serviceInstaller.ServiceName = service.ServiceName;
                    serviceInstaller.Uninstall(null);

                    await UIService.Log_MensagemAsync($"Serviço: {service.DisplayName} - Deleted", true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                UIService.Erro++;
                await UIService.Log_MensagemAsync($"Erro ao desinstalar o Serviço: {service.DisplayName}\n{ex.Message}", true);
                return false;
            }
        }
    }
}
