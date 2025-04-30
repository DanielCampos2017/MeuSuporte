using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks; 

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
                await createPath(diretorio);

                var processStartInfo = CreateProcessStartInfo(diretorio);

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    ProcessDataReceived(process);

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    await WaitForExitAsync(process);
                }                    

                _MainForm.Sucesso++;
                await _MainForm.Log_MensagemAsync("Backup Driver: Criado com Sucesso", true);
            }
            catch (Exception ex)
            {
                _MainForm.Erro++;
                await _MainForm.Log_MensagemAsync("Backup Driver: Erro - " + ex.Message, true);
            }
        }

        // Cria o Diretorio
        private async Task createPath(string caminho)
        {
            if (!Directory.Exists(caminho))
            {
                Directory.CreateDirectory(caminho);
            }
        }

        //Cria um processo para executado
        private ProcessStartInfo CreateProcessStartInfo(string diretorio)
        {
            string argumento = $"dism /online /export-driver /destination:\"{diretorio}\"";

            return new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C {argumento}",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Verb = "runas"
            };
        }

        //Trata a saida do Processo
        private void ProcessDataReceived(Process process)
        {
            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    _MainForm.Invoke(new Action(() =>
                    {
                        //Task task = _MainForm.DriverBackup_ExtractProgress(e.Data);
                        Task task = DriverBackup_ExtractProgress(e.Data);
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
        }

        // aguarda a saída do processo de forma assíncrona. para evita que a thread atual seja bloqueada.
        private Task WaitForExitAsync(Process process)
        {
            return Task.Run(() => process.WaitForExit());
        }

        // Trata do Dados saido do Processo
        public async Task DriverBackup_ExtractProgress(string valor)
        {
            // Verifica se a string contém a palavra "Exportando"
            //valor esperado = Exportando 10 de 74 - oem.inf: O pacote de driver foi exportado com sucesso."

            if (!valor.Contains("Exportando"))
            {
                return;
            }

            // Expressão regular para capturar números
            MatchCollection matches = Regex.Matches(valor, @"\d+");

            int valorMaximo = (matches.Count > 1) ? Convert.ToInt32(matches[1].Value) : 100;

            float unidade = (float)_MainForm.ValueUniProgressBar / (float)valorMaximo;

            _MainForm.ProgressBarADD(await ValueUnit(unidade));
        }

        // Funcao que envia a porcentagem para ProgressBar
        private static float accumulator = 0f; // Variável para armazenar o valor acumulado    
        private static async Task<int> ValueUnit(float valor)
        {
            accumulator += valor;

            // Extrai a parte inteira e armazena na ProgressBar
            int parteInteira = (int)accumulator;

            if (parteInteira > 0)
            {
                accumulator -= parteInteira;
                return parteInteira;
            }
            return 0;
        }

    }
}
