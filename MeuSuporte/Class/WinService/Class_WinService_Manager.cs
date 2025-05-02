using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_WinService_Manager
    {
        private MainForm _MainForm;
        Class_WinService_Uninstall WinService_Uninstall;
        Class_WinService_Disabled WinService_Disabled;
        public Class_WinService_Manager(MainForm Form_)
        {
            _MainForm = Form_;
            WinService_Uninstall = new Class_WinService_Uninstall(_MainForm);
            WinService_Disabled = new Class_WinService_Disabled(_MainForm);
        }

        public async Task Manager(string[] ListService, int ValueUniProgressBar, bool isDisableService, CancellationToken token)
        {    
            token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

            //var uninstallService = new Class_WinService_Uninstall(_MainForm);
            //var serviceDisabled = new Class_WinService_Disabled(_MainForm);

            int total = ListService.Length;
            float valorUnidade = (float)ValueUniProgressBar / total;
            bool foundServices = false;

            foreach (string serviceName in ListService)
            {
                token.ThrowIfCancellationRequested();

                var service = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == serviceName);

                int NewValor = await ValueUnit(valorUnidade);
                if (NewValor >= 1)
                {
                    _MainForm.ProgressBarADD(NewValor);
                    await Task.Delay(20);
                }

                if (!isDisableService & service != null)
                {
                    await WinService_Uninstall.TryUninstallAsync(service);
                    foundServices = true;
                }

                if (isDisableService & service != null)
                {
                    await WinService_Disabled.WaitForServiceToDisabled(service);
                    foundServices = true;
                }
            }

            if (!foundServices)
            {
                _MainForm.ProgressBarADD(ValueUniProgressBar);
                _MainForm.Log_MensagemAsync("Serviço: No listings found!", true);
                await Task.Delay(500);
            }
        }

        // Funcao que envia a porcentagem para ProgressBar
        private static float accumulator = 0f; // Variável para armazenar o valor acumulado    
        private static async Task<int> ValueUnit(float valor)
        {
            accumulator += valor;

            // Extrai a parte inteira e armazena na ProgressBar
            int parteInteira = (int)accumulator;

            if (parteInteira > 0)
            {
                accumulator -= parteInteira;
                return parteInteira;
            }
            return 0;
        }

    }
}
