﻿using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinTrash_Mananger
    {
        
        // Importação do método nativo para limpar a lixeira
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        private static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleBinFlags dwFlags);


        [Flags]
        enum RecycleBinFlags
        {
            NoConfirmation = 0x00000001, // Sem confirmacao
            NoProgressUI = 0x00000002, // Sem barra de progresso
            NoSound = 0x00000004 // Sem som
        }
        
        public async Task Mananger()
        {
            try
            {
                WinGlobal_UIService.Instance.token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                WinGlobal_UIService.Instance.ProgressBarADD(WinGlobal_UIService.Instance.ValueUniProgressBar / 2);                

                await WinGlobal_UIService.Instance.Log_MensagemAsync("Lixeira: Apagando...", true);
                await Task.Delay(500);

                uint result = await Task.Run(() =>
                    SHEmptyRecycleBin(IntPtr.Zero, null,
                    RecycleBinFlags.NoConfirmation | RecycleBinFlags.NoProgressUI | RecycleBinFlags.NoSound)
                );

                if (result == 0) // Se o resultado for 0, sucesso na exclusão
                {
                    WinGlobal_UIService.Instance.Sucesso++;
                    await WinGlobal_UIService.Instance.Log_MensagemAsync("Lixeira: Apagada!", true);
                    await Task.Delay(500);
                    WinGlobal_UIService.Instance.ProgressBarADD(WinGlobal_UIService.Instance.ValueUniProgressBar / 2);
                }

                if (result == 2147549183) // Lixeira já estava vazia
                {
                    WinGlobal_UIService.Instance.Sucesso++;
                    await WinGlobal_UIService.Instance.Log_MensagemAsync("Lixeira: Já estava vazia", true);
                    await Task.Delay(500);
                    WinGlobal_UIService.Instance.ProgressBarADD(WinGlobal_UIService.Instance.ValueUniProgressBar / 2);
                }         
            }
            catch (Exception e)
            {
                WinGlobal_UIService.Instance.Erro++;
               await WinGlobal_UIService.Instance.Log_MensagemAsync($"Lixeira: Erro ao apagar. Detalhes: \n  {e.Message}", true);
            }
        }
    }
}
