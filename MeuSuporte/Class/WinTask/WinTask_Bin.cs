using System;
using System.Threading;
using System.Threading.Tasks;
using TaskScheduler;

namespace MeuSuporte
{
    internal class WinTask_Bin
    {        
        public async Task Delete(ITaskFolder rootFolder, IRegisteredTask task, CancellationToken token, int valor)
        {
            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                rootFolder.DeleteTask(task.Name, 0); // deleta a tarefa
                await WinGlobal_UIService2.Instance.Log_MensagemAsync($"Tarefa Apagada: {task.Name}", true);
                await Task.Delay(500);
            }
            catch (Exception e)
            {
                WinGlobal_UIService2.Instance.Erro++;
                await WinGlobal_UIService2.Instance.Log_MensagemAsync($"Erro ao Apagar Tarefa: {task.Name} - {e.Message}", true);
                await Task.Delay(500);
            }
            WinGlobal_UIService2.Instance.ProgressBarADD(valor);
        }
    }
}
