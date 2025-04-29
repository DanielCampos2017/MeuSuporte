using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_BackupBootBCD
    {
        private MainForm _MainForm;    

        public Class_BackupBootBCD(MainForm Form_)
        {
            _MainForm = Form_;
        }

        public async Task Backup(CancellationToken token, int ValueUniProgressBar)
        {            
            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
   
                Class_GeraNomePasta _Class_GeraNomePasta = new Class_GeraNomePasta();
                string _directory = _Class_GeraNomePasta.Directory();

                _directory += "Backup Boot BCD";
                _MainForm.ProgressBarADD(ValueUniProgressBar / 2);

                if (!Directory.Exists(_directory))
                {
                    Directory.CreateDirectory(_directory);
                }

                string _arguments = "bcdedit /export " + '"' + _directory + "\\BCD_Backup.bcd" + '"';

                ProcessStartInfo proccessStartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {_arguments}",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    Verb = "runas" // Isso pedirá permissões de administrador
                };

                using (Process process = Process.Start(proccessStartInfo))
                {
                    if (process != null)
                    {
                        await WaitForExitAsync(process);
                        _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
                        await _MainForm.Log_MensagemAsync("Backup Boot BCD: Criado com Sucesso", true);
                        await Task.Delay(500);
                    }
                }
                _MainForm.Sucesso++;
            }
            catch (Exception ex)
            {
                _MainForm.Erro++;
                await _MainForm.Log_MensagemAsync("Backup Boot BCD: Erro - " + ex.Message, true);
                await Task.Delay(500);
            }
        }

        private async Task WaitForExitAsync(Process process)
        {
            // Executa a tarefa async em uma nova thread
            await Task.Run(() => process.WaitForExit());
        }
    }
    
    
    //Abra as Opções avançadas de inicialização
    //Execute o comando Bootrec /ScanOS para verificar os sistemas instalados
    //Reinicie o computador
    //Se o problema persistir, execute os seguintes comandos:
    //bcdedit /export c:\bcdbackup
    //attrib c:\boot\bcd -r -s -h
    //ren c:\boot\bcd bcd.old
    //bootrec /rebuildbcd
    //Reinicie o sistema
    //Se a ferramenta do Bootrec.exe não conseguir localizar uma instalação do Windows, pode ser necessário remover e recriar o BCD store. 
    //O BCDBoot é uma ferramenta de linha de comando que configura os arquivos de inicialização para executar o sistema operacional Windows. O arquivo de registro BCD está localizado no diretório //\Boot\Bcd da partição ativa. 
    
}
