using System;
using System.Threading;
using System.Threading.Tasks;
using TaskScheduler;

namespace MeuSuporte
{
    internal class Class_WinTask_Bin
    {
        private MainForm _MainForm;

        public Class_WinTask_Bin(MainForm Form_)
        {
            _MainForm = Form_;
        }

        public async Task Delete(ITaskFolder rootFolder, IRegisteredTask task, CancellationToken token, int valor)
        {
            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                rootFolder.DeleteTask(task.Name, 0); // deleta a tarefa
                await _MainForm.Log_MensagemAsync($"Tarefa Apagada: {task.Name}", true);
                await Task.Delay(500);
            }
            catch (Exception e)
            {
                _MainForm.Erro++;
                await _MainForm.Log_MensagemAsync($"Erro ao Apagar Tarefa: {task.Name} - {e.Message}", true);
                await Task.Delay(500);
            }
            _MainForm.ProgressBarADD(valor);
        }
    }
}
