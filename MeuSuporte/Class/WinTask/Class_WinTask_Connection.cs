using System.Threading.Tasks;
using TaskScheduler;

namespace MeuSuporte
{
    internal class Class_WinTask_Connection
    {
        public ITaskService taskService;
        public ITaskFolder rootFolder;
        public IRegisteredTaskCollection tasks;

        public async Task Connect()
        {
            taskService = new TaskScheduler.TaskScheduler();// cria uma instância
            taskService.Connect(); // conecta

            rootFolder = taskService.GetFolder(@"\"); // passa o diretório raiz
            tasks = rootFolder.GetTasks(0);
        }
    }
}
