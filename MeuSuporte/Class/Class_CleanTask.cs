using MeuSuporte.Properties;
using System;
using System.Threading.Tasks;
using TaskScheduler;

namespace TaskScheduler
{
    internal class Class_CleanTask
    {
        private MainForm _MainForm;

        public Class_CleanTask(MainForm formPreventiva)
        {
            _MainForm = formPreventiva;
        }

        public async Task DeletaTarefa()
        {
            if (_MainForm.AbortExecution)
            {
                return;
            }

            try
            {
            ///    _formPreventiva.PainelInfoDescricao(Resources.CleanTask, "Limpar Tarefas Agendadas:\n\rRemove tarefas desnecessárias programadas no sistema, melhorando o desempenho.");

                ITaskService taskService = new TaskScheduler(); // cria uma instância
                taskService.Connect(); // conecta

                ITaskFolder rootFolder = taskService.GetFolder(@"\"); // passa o diretório raiz
                IRegisteredTaskCollection tasks = rootFolder.GetTasks(0);

                if (tasks.Count == 0)
                {
                    _MainForm.Log_Mensagem("Nenhuma Tarefa foi encontrada ", "");
                    return;
                }

                foreach (IRegisteredTask task in tasks) // Verifica a quantidade de tarefas no diretório
                {
                    // Use Task.Run para executar a exclusão de forma assíncrona e evitar travamentos na UI
                    await Task.Run(() =>
                    {
                        try
                        {
                            rootFolder.DeleteTask(task.Name, 0); // deleta a tarefa
                            _MainForm.Log_Mensagem("Tarefa Apagada:", task.Name);
                        }
                        catch (Exception e)
                        {
                            _MainForm.Erro++;
                            _MainForm.Log_Mensagem($"Erro ao Apagar Tarefa: {task.Name} - {e.Message}","");
                        }
                    });
                }
                _MainForm.Sucesso++;
            }
            catch (Exception ex)
            {
                _MainForm.Erro++;
                _MainForm.Log_Mensagem("Erro ao Apagar Tarefa:", ex.Message);
            }
        }
    }
}
