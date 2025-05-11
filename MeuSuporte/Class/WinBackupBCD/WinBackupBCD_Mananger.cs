using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBackupBCD_Mananger
    {
        private readonly WinGlobal_UIService UIService;
        WinGlobal_DirectoryMananger DirectoryManange;
        WinBackupBCD_ProcessInfo BCD_ProcessInfo;

        public WinBackupBCD_Mananger(WinGlobal_UIService ui)
        {
            UIService = ui;
            DirectoryManange = new WinGlobal_DirectoryMananger();
            BCD_ProcessInfo = new WinBackupBCD_ProcessInfo();
        }

        public async Task Mananger(CancellationToken token, int ValueUniProgressBar)
        {
            string NameFolder = "BCD_Backup";

            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
   
                // Cria o diretorio
                if (DirectoryManange.Create(NameFolder) == false)
                {
                    await UIService.Log_MensagemAsync($"Ocorreu um erro ao tentar criar Pasta {NameFolder}", true);
                    return;
                }

                UIService.ProgressBarADD(ValueUniProgressBar / 2);

                //Cria um processo para executar
                var processStartInfo = BCD_ProcessInfo.Create(DirectoryManange.GetDirectory(NameFolder));

                using (var process = new Process { StartInfo = await processStartInfo.ConfigureAwait(false) })               
                {
                    process.Start();                   
                    await WaitForExitAsync(process);
                    await Task.Delay(500);
                    UIService.ProgressBarADD(ValueUniProgressBar / 2);
                    await UIService.Log_MensagemAsync("Backup Boot BCD: Criado com Sucesso", true);
                }
                UIService.Sucesso++;
            }
            catch (Exception ex)
            {
                UIService.Erro++;
                await UIService.Log_MensagemAsync("Backup Boot BCD: Erro - " + ex.Message, true);
                await Task.Delay(500);
            }
        }

        // Executa a tarefa async em uma nova thread
        private async Task WaitForExitAsync(Process process)
        {
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
