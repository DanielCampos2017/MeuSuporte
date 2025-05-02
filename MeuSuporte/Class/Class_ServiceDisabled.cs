using System.ServiceProcess;
using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MeuSuporte
{
    internal class Class_ServiceDisabled
    {
        private readonly MainForm _MainForm;
        private readonly Class_ServiceStop _serviceStopper;

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool ChangeServiceConfig(
    IntPtr hService,
    uint nServiceType,
    uint nStartType,
    uint nErrorControl,
    string lpBinaryPathName,
    string lpLoadOrderGroup,
    IntPtr lpdwTagId,
    [In] char[] lpDependencies,
    string lpServiceStartName,
    string lpPassword,
    string lpDisplayName);

        private const uint SERVICE_NO_CHANGE = 0xFFFFFFFF;

        public Class_ServiceDisabled(MainForm mainForm)
        {
            _MainForm = mainForm;
            _serviceStopper = new Class_ServiceStop(mainForm); // criada apenas uma vez
        }

        public async Task WaitForServiceToDisabled(ServiceController service)
        {
            bool isServiceStopped = await _serviceStopper.WaitForServiceToStop(service); // Para o serviço   

            if (!isServiceStopped)
            {
                _MainForm.Log_MensagemAsync($"Serviço: {service.DisplayName} não pode ser desabilitado devido ainda está em execução ", true);
                await Task.Delay(500);
                _MainForm.Erro++;
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
                            SERVICE_NO_CHANGE,
                            (uint)ServiceStartMode.Disabled,
                            SERVICE_NO_CHANGE,
                            null,
                            null,
                            IntPtr.Zero,
                            null,
                            null,
                            null,
                            null);
                    }

                    _MainForm.Log_MensagemAsync($"Serviço:  {service.DisplayName} - Disabled", true);
                    await Task.Delay(500);
                    _MainForm.Sucesso++;
                }
                catch (Exception ex)
                {
                    _MainForm.Log_MensagemAsync($"Erro ao Tentar mudar o Modo de Inicialização do Serviço: {service.DisplayName} " + ex.Message, true);
                    await Task.Delay(500);
                    _MainForm.Erro++;
                }
            }

        }
    }
}
