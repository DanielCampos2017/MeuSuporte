using System;
using System.Threading;
using System.Threading.Tasks;
using TaskScheduler;

namespace MeuSuporte
{
    internal class Class_WinTask_Manager
    {
        private MainForm _MainForm;
        private Class_WinTask_Bin WinTask_Bin;
        private Class_WinTask_Connection WinTask_Connection;

        public Class_WinTask_Manager(MainForm Form_)
        {
            _MainForm = Form_;
            WinTask_Bin = new Class_WinTask_Bin(_MainForm);
            WinTask_Connection = new Class_WinTask_Connection();
        }
              
        public async Task Manager(CancellationToken token, int ValueUniProgressBar)
        {
            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                await WinTask_Connection.Connect();

                // verifica se existe tarefas
                if (WinTask_Connection.tasks.Count == 0)
                {
                    await _MainForm.Log_MensagemAsync("Nenhuma Tarefa foi encontrada ", true);
                    _MainForm.ProgressBarADD(ValueUniProgressBar);
                    return;
                }

                int valor = ValueUniProgressBar / WinTask_Connection.tasks.Count;

                foreach (IRegisteredTask task in WinTask_Connection.tasks) // Verifica a quantidade de tarefas no diretório
                {
                    await WinTask_Bin.Delete(WinTask_Connection.rootFolder, task, token, valor); // apaga a tarefa
                }
                _MainForm.Sucesso++;
            }
            catch (Exception ex)
            {
                _MainForm.Erro++;
                await _MainForm.Log_MensagemAsync($"Gerou um erro na execução Clean Task\r\n: {ex.Message}", true);
            }
        }
    }
}
