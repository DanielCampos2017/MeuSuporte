using System;
using System.Threading;
using System.Threading.Tasks;
using TaskScheduler;

namespace MeuSuporte
{
    internal class WinTask_Mananger
    {
        private readonly WinGlobal_UIService UIService;
        private WinTask_Bin WinTask_Bin;
        private WinTask_Connection WinTask_Connection;

        public WinTask_Mananger(WinGlobal_UIService ui)
        {
            UIService = ui;
            WinTask_Bin = new WinTask_Bin(UIService);
            WinTask_Connection = new WinTask_Connection();
        }
              
        public async Task Mananger(CancellationToken token, int ValueUniProgressBar)
        {
            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                await WinTask_Connection.Connect();

                // verifica se existe tarefas
                if (WinTask_Connection.tasks.Count == 0)
                {
                    await UIService.Log_MensagemAsync("Nenhuma Tarefa foi encontrada ", true);
                    UIService.ProgressBarADD(ValueUniProgressBar);
                    return;
                }

                int valor = ValueUniProgressBar / WinTask_Connection.tasks.Count;

                foreach (IRegisteredTask task in WinTask_Connection.tasks) // Verifica a quantidade de tarefas no diretório
                {
                    await WinTask_Bin.Delete(WinTask_Connection.rootFolder, task, token, valor); // apaga a tarefa
                }
                UIService.Sucesso++;
            }
            catch (Exception ex)
            {
                UIService.Erro++;
                await UIService.Log_MensagemAsync($"Gerou um erro na execução Clean Task\r\n: {ex.Message}", true);
            }
        }
    }
}
