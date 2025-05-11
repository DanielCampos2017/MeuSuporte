using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinTrash_Mananger
    {
        private readonly WinGlobal_UIService UIService;

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
        public WinTrash_Mananger(WinGlobal_UIService ui)
        {
            UIService = ui;
        }

        public async Task Mananger(CancellationToken token, int ValueUniProgressBar)
        {
            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                UIService.ProgressBarADD(ValueUniProgressBar / 2);                

                await UIService.Log_MensagemAsync("Lixeira: Apagando...", true);
                await Task.Delay(500);

                uint result = await Task.Run(() =>
                    SHEmptyRecycleBin(IntPtr.Zero, null,
                    RecycleBinFlags.NoConfirmation | RecycleBinFlags.NoProgressUI | RecycleBinFlags.NoSound), token
                );

                if (result == 0) // Se o resultado for 0, sucesso na exclusão
                {
                    UIService.Sucesso++;
                    await UIService.Log_MensagemAsync("Lixeira: Apagada!", true);
                    await Task.Delay(500);
                    UIService.ProgressBarADD(ValueUniProgressBar / 2);
                }

                if (result == 2147549183) // Lixeira já estava vazia
                {
                    UIService.Sucesso++;
                    await UIService.Log_MensagemAsync("Lixeira: Já estava vazia", true);
                    await Task.Delay(500);
                    UIService.ProgressBarADD(ValueUniProgressBar / 2);
                }         
            }
            catch (Exception e)
            {
                UIService.Erro++;
               await UIService.Log_MensagemAsync($"Lixeira: Erro ao apagar. Detalhes: \n  {e.Message}", true);
            }
        }
    }
}
