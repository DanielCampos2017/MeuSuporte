﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBackupDriver_Mananger
    {
        private  WinGlobal_DirectoryMananger DirectoryManange;
        private  WinBackupDriver_ProcessInfo _ProcessInfo;
        private  WinBackupDriver_DataReceived _DataReceived;
            
        public async Task Mananger()
        {
            DirectoryManange = new WinGlobal_DirectoryMananger();
            _ProcessInfo = new WinBackupDriver_ProcessInfo();
            _DataReceived = new WinBackupDriver_DataReceived();
            string NameFolder = "DriversBackup";

            await WinGlobal_UIService.Instance.Log_MensagemAsync("Backup Driver: executando...", true);         

            try
            {
                WinGlobal_UIService.Instance.token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar           
                
                // Cria o diretorio
                if (DirectoryManange.Create(NameFolder) == false)
                {
                    await WinGlobal_UIService.Instance.Log_MensagemAsync($"Ocorreu um erro ao tentar criar Pasta {NameFolder}", true);
                    return;
                }
                                

                //Cria um processo para executar
                var processStartInfo = _ProcessInfo.Create(DirectoryManange.GetDirectory(NameFolder));
               

                using (var process = new Process { StartInfo = await processStartInfo.ConfigureAwait(false) })
                {
                    _DataReceived.Received(process);   
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    await WaitForExitAsync(process);
                }

                WinGlobal_UIService.Instance.Sucesso++;
                await WinGlobal_UIService.Instance.Log_MensagemAsync("Backup Driver: Criado com Sucesso", true);
            }
            catch (Exception ex)
            {
                WinGlobal_UIService.Instance.Erro++;
                await WinGlobal_UIService.Instance.Log_MensagemAsync("Backup Driver: Erro - " + ex.Message, true);
            }
        }

        // aguarda a saída do processo de forma assíncrona
        private Task WaitForExitAsync(Process process)
        {
            return Task.Run(() => process.WaitForExit());
        }
     
    }
}
