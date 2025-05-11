using System;
using System.Threading;
using System.Threading.Tasks;
using TaskScheduler;

namespace MeuSuporte
{
    internal class WinTask_Bin
    {
        private readonly WinGlobal_UIService UIService;

        public WinTask_Bin(WinGlobal_UIService ui)
        {
            UIService = ui;
        }

        public async Task Delete(ITaskFolder rootFolder, IRegisteredTask task, CancellationToken token, int valor)
        {
            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                rootFolder.DeleteTask(task.Name, 0); // deleta a tarefa
                await UIService.Log_MensagemAsync($"Tarefa Apagada: {task.Name}", true);
                await Task.Delay(500);
            }
            catch (Exception e)
            {
                UIService.Erro++;
                await UIService.Log_MensagemAsync($"Erro ao Apagar Tarefa: {task.Name} - {e.Message}", true);
                await Task.Delay(500);
            }
            UIService.ProgressBarADD(valor);
        }
    }
}
