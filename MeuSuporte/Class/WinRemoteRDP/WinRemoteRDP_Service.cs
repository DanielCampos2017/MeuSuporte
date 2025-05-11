using System;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinRemoteRDP_Service
    {
        public async Task ServiceEnable()
        {
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
        }
    }
}
