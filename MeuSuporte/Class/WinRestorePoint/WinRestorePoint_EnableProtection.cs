﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinRestorePoint_EnableProtection
    {
        public async Task EnableProtection(int ValueUniProgressBar )
        {
            try
            {
                // Habilita a Proteção do Sistema no disco C:
                await RunCommandAsync("powershell", "Enable-ComputerRestore -Drive C:\\");

                // Redimensiona o espaço de armazenamento da sombra para 10%
                await RunCommandAsync("powershell", "-Command \"& 'C:\\Windows\\System32\\vssadmin.exe' resize shadowstorage /for=C: /on=C: /maxsize=10%\"");
               
                await WinGlobal_UIService.Instance.Log_MensagemAsync(@"Configuração de proteção do sistema foi ativado", true);

                WinGlobal_UIService.Instance.ProgressBarADD(ValueUniProgressBar);
            }      
            catch (Exception ex)
            {
                WinGlobal_UIService.Instance.Erro++;
                await WinGlobal_UIService.Instance.Log_MensagemAsync(@"Ocorreu um erro ao tentar ativar a configuração de Proteção do sistema", true);
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
