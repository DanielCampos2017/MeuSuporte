using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks; // Adicionado para métodos assíncronos

namespace MeuSuporte
{
    internal class Class_BackupDriver
    {
        private MainForm _MainForm;
        private Class_GeraNomePasta _Class_GeraNomePasta;

        public Class_BackupDriver(MainForm Form_)
        {
            _MainForm = Form_;
        }

      
        public async Task Backup(CancellationToken token)
        {
            await _MainForm.Log_MensagemAsync("Backup Driver: executando...", true);
            _Class_GeraNomePasta = new Class_GeraNomePasta();

            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                string diretorio = _Class_GeraNomePasta.Directory() + @"DriversBackup";

                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                }

                string argumento = $"dism /online /export-driver /destination:\"{diretorio}\"";

                ProcessStartInfo proccessStartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {argumento}",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true, // Captura a saída padrão
                    RedirectStandardError = true,  // Captura erros
                    Verb = "runas"
                };

                using (Process process = new Process { StartInfo = proccessStartInfo })
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            _MainForm.Invoke(new Action(() => {
                                //_MainForm.Log_MensagemAsync("DISM: " + e.Data, true);
                                Task task = _MainForm.DriverBackup_ExtractProgress(e.Data);
                            }));
                        }
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            _MainForm.Erro++;
                            Task task = _MainForm.Log_MensagemAsync("Erro DISM: " + e.Data, true);
                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine(); // Inicia a leitura da saída
                    process.BeginErrorReadLine();  // Inicia a leitura dos erros
                    await WaitForExitAsync(process); // Método manual para aguardar a saída
                }

                _MainForm.Sucesso++;
                _MainForm.Log_MensagemAsync("Backup Driver: Criado com Sucesso", true);
            }
            catch (Exception ex)
            {
                _MainForm.Erro++;
                Task task = _MainForm.Log_MensagemAsync("Backup Driver: Erro - " + ex.Message, true);
            }
        }

        private Task WaitForExitAsync(Process process)
        {
            return Task.Run(() => process.WaitForExit());
        }

    }
}
