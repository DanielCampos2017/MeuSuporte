using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using MeuSuporte;
using TaskScheduler;

namespace TaskScheduler 
{
    internal class Class_CleanTask
    {
        private MainForm _MainForm;

        public Class_CleanTask(MainForm Form_)
        {
            _MainForm = Form_;
        }
              
        public async Task DeletaTarefa(CancellationToken token, int ValueUniProgressBar)
        {
            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                ITaskService taskService = new TaskScheduler(); // cria uma instância
                taskService.Connect(); // conecta

                ITaskFolder rootFolder = taskService.GetFolder(@"\"); // passa o diretório raiz
                IRegisteredTaskCollection tasks = rootFolder.GetTasks(0);
                  
                if (tasks.Count == 0)
                {
                    await _MainForm.Log_MensagemAsync("Nenhuma Tarefa foi encontrada ", true);
                    await Task.Delay(500);
                    _MainForm.ProgressBarADD(ValueUniProgressBar);
                    return;
                }
                int valor = ValueUniProgressBar / tasks.Count;

                foreach (IRegisteredTask task in tasks) // Verifica a quantidade de tarefas no diretório
                {
                    await DeleteTask(rootFolder, task, token, valor);
                }
                _MainForm.Sucesso++;
            }
            catch (Exception ex)
            {
                _MainForm.Erro++;
                await _MainForm.Log_MensagemAsync($"Gerou um erro na execução Clean Task\r\n: {ex.Message}", true);
                await Task.Delay(500);
            }
        }

        private async Task DeleteTask(ITaskFolder rootFolder, IRegisteredTask task, CancellationToken token, int valor)
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
