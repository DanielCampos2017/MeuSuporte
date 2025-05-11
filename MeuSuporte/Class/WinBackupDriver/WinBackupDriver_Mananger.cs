using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBackupDriver_Mananger
    {
        private readonly WinGlobal_UIService UIService;
        WinGlobal_DirectoryMananger DirectoryManange;
        WinBackupDriver_ProcessInfo  WinBackupDriver_ProcessInfo;        
        WinBackupDriver_DataReceived _DataReceived;
                
        public WinBackupDriver_Mananger(WinGlobal_UIService ui)
        {
            UIService = ui;
            DirectoryManange = new WinGlobal_DirectoryMananger();
            WinBackupDriver_ProcessInfo = new WinBackupDriver_ProcessInfo();
            _DataReceived = new WinBackupDriver_DataReceived(UIService);
        }

        public async Task Mananger(CancellationToken token)
        {
            string NameFolder = "DriversBackup";

            await UIService.Log_MensagemAsync("Backup Driver: executando...", true);         

            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar           
                
                // Cria o diretorio
                if (DirectoryManange.Create(NameFolder) == false)
                {
                    await UIService.Log_MensagemAsync($"Ocorreu um erro ao tentar criar Pasta {NameFolder}", true);
                    return;
                }
                                

                //Cria um processo para executar
                var processStartInfo = WinBackupDriver_ProcessInfo.Create(DirectoryManange.GetDirectory(NameFolder));
               

                using (var process = new Process { StartInfo = await processStartInfo.ConfigureAwait(false) })
                {
                    _DataReceived.Received(process);              

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    await WaitForExitAsync(process);
                }

                UIService.Sucesso++;
                await UIService.Log_MensagemAsync("Backup Driver: Criado com Sucesso", true);
            }
            catch (Exception ex)
            {
                UIService.Erro++;
                await UIService.Log_MensagemAsync("Backup Driver: Erro - " + ex.Message, true);
            }
        }

        // aguarda a saída do processo de forma assíncrona
        private Task WaitForExitAsync(Process process)
        {
            return Task.Run(() => process.WaitForExit());
        }
     
    }
}
