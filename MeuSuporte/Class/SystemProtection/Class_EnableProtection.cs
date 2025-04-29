using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_EnableProtection
    {
        private MainForm _MainForm;

        public Class_EnableProtection(MainForm Form_)
        {
            _MainForm = Form_;
        }

        public async Task EnableProtection(int ValueUniProgressBar )
        {
            try
            {
                // Habilita a Proteção do Sistema no disco C:
                await RunCommandAsync("powershell", "Enable-ComputerRestore -Drive C:\\");

                // Redimensiona o espaço de armazenamento da sombra para 10%
                await RunCommandAsync("powershell", "-Command \"& 'C:\\Windows\\System32\\vssadmin.exe' resize shadowstorage /for=C: /on=C: /maxsize=10%\"");
               
                await _MainForm.Log_MensagemAsync(@"Configuração de proteção do sistema foi ativado", true);

                _MainForm.ProgressBarADD(ValueUniProgressBar);
            }      
            catch (Exception ex)
            {
                _MainForm.Erro++;
                await _MainForm.Log_MensagemAsync(@"Ocorreu um erro ao tentar ativar a configuração de Proteção do sistema", true);
            }
        }

        private async Task RunCommandAsync(string filename, string arguments)
        {
            await Task.Run(() =>
            {
                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    FileName = filename,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Verb = "runas" // Executa como Administrador
                };

                using (Process process = new Process() { StartInfo = psi })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (!string.IsNullOrWhiteSpace(output))
                        Console.WriteLine("Saída: " + output);
                    if (!string.IsNullOrWhiteSpace(error))
                        Console.WriteLine("Erro: " + error);
                }
            });
        }















    }
}
