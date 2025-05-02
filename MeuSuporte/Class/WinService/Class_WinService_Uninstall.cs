using System;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_WinService_Uninstall
    {
        private readonly MainForm _mainForm;
        private readonly Class_WinService_Stop _serviceStopper;

        public Class_WinService_Uninstall(MainForm mainForm)
        {
            _mainForm = mainForm;
            _serviceStopper = new Class_WinService_Stop(mainForm); // criada apenas uma vez
        }

        public async Task<bool> TryUninstallAsync(ServiceController service)
        {

            bool isServiceStopped = await _serviceStopper.WaitForServiceToStop(service);

            if (!isServiceStopped)
            {
                await _mainForm.Log_MensagemAsync($"Serviço: {service.DisplayName} não pode ser removido pois ainda está em execução.", true);
                return false;
            }

            try
            {
                using (var serviceInstaller = new ServiceInstaller())
                {
                    serviceInstaller.Context = new InstallContext(null, null);
                    serviceInstaller.ServiceName = service.ServiceName;
                    serviceInstaller.Uninstall(null);

                    await _mainForm.Log_MensagemAsync($"Serviço: {service.DisplayName} - Deleted", true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _mainForm.Erro++;
                await _mainForm.Log_MensagemAsync($"Erro ao desinstalar o Serviço: {service.DisplayName}\n{ex.Message}", true);
                return false;
            }
        }
    }
}
