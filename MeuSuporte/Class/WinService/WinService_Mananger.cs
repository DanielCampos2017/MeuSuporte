using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinService_Mananger
    {
        private readonly WinGlobal_UIService UIService;
        WinService_Uninstall WinService_Uninstall;
        WinService_Disabled WinService_Disabled;
        public WinService_Mananger(WinGlobal_UIService ui)
        {
            UIService = ui;
            WinService_Uninstall = new WinService_Uninstall(UIService);
            WinService_Disabled = new WinService_Disabled(UIService);
        }

        public async Task Mananger(string[] ListService, int ValueUniProgressBar, bool isDisableService, CancellationToken token)
        {    
            token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

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
                    UIService.ProgressBarADD(NewValor);
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
                UIService.ProgressBarADD(ValueUniProgressBar);
                UIService.Log_MensagemAsync("Serviço: No listings found!", true);
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
