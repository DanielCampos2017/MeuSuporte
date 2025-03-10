using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace TaskScheduler
{
    internal class Class_BackupBootBCD
    {
        private MainForm _MainForm;
        private Class_GeraNomePasta _Class_GeraNomePasta;

        public Class_BackupBootBCD(MainForm formPreventiva)
        {
            _MainForm = formPreventiva;
        }

        public async Task Backup()
        {
            if (_MainForm.AbortExecution)
            {
                return;
            }

            try
            {
                _Class_GeraNomePasta = new Class_GeraNomePasta();
                string _Directory = _Class_GeraNomePasta.Directory();

                _Directory += "Backup Boot BCD";

                if (!Directory.Exists(_Directory))
                {
                    Directory.CreateDirectory(_Directory);
                }

                string arqumento = "bcdedit /export " + '"' + _Directory + "\\BCD_Backup.bcd" + '"';

                ProcessStartInfo proccessStartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {arqumento}",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    Verb = "runas" // Isso pedirá permissões de administrador
                };

                using (Process process = Process.Start(proccessStartInfo))
                {
                    if (process != null)
                    {
                        await WaitForExitAsync(process);
                         _MainForm.Log_Mensagem("Backup Boot BCD: Criado com Sucesso", " ");
                    }
                }
                _MainForm.Sucesso++;
            }
            catch (Exception ex)
            {
                _MainForm.Erro++;
                _MainForm.Log_Mensagem("Backup Boot BCD: Erro - " + ex.Message, " ");
            }
        }

        private async Task WaitForExitAsync(Process process)
        {
            // Cria uma Task que aguarda o processo terminar
            await Task.Run(() => process.WaitForExit());
        }
    }
}
