using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks; // Adicionado para métodos assíncronos

namespace TaskScheduler
{
    internal class Class_BackupDriver
    {
        private MainForm _MainForm;
        private Class_GeraNomePasta _Class_GeraNomePasta;

        public Class_BackupDriver(MainForm formPreventiva)
        {
            _MainForm = formPreventiva;
        }

        public async Task Backup() // Agora é async Task
        {
            if (_MainForm.AbortExecution)
            {
                return;
            }
            //  _formPreventiva.Log_Mensagem("Backup Driver: executando...", " ");

            _Class_GeraNomePasta = new Class_GeraNomePasta();
            try
            {
                string diretorio = _Class_GeraNomePasta.Directory();
                diretorio += @"DriversBackup";

                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                }

                string argumento = "dism /online /export-driver /destination:" + '"' + diretorio + '"';

                ProcessStartInfo proccessStartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {argumento}",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    Verb = "runas"
                };

                using (Process process = Process.Start(proccessStartInfo))
                {
                    if (process != null)
                    {
                        await WaitForExitAsync(process); // Chama a função para aguardar o término sem bloquear a UI
                    }
                }
                _MainForm.Sucesso++;
                _MainForm.Log_Mensagem("Backup Driver: Criado com Sucesso", " ");
            }
            catch (Exception ex)
            {
                _MainForm.Erro++;
                _MainForm.Log_Mensagem("Backup Driver: Erro - " + ex.Message, " ");
            }
        }

        // Função personalizada para aguardar a finalização do processo de forma assíncrona
        private Task WaitForExitAsync(Process process)
        {
            var tcs = new TaskCompletionSource<object>();

            // Evento que é disparado quando o processo termina
            process.Exited += (sender, args) => tcs.SetResult(null);
            process.EnableRaisingEvents = true; // Habilita eventos

            return tcs.Task;
        }
    }
}
