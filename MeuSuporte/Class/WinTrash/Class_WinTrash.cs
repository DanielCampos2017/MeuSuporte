using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_WinTrash
    {
        private MainForm _MainForm;

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
        public Class_WinTrash(MainForm Form_)
        {
            _MainForm = Form_;
        }

        public async Task Clear(CancellationToken token, int ValueUniProgressBar)
        {
            try
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                _MainForm.ProgressBarADD(ValueUniProgressBar / 2);                

                await _MainForm.Log_MensagemAsync("Lixeira: Apagando...", true);
                await Task.Delay(500);

                uint result = await Task.Run(() =>
                    SHEmptyRecycleBin(IntPtr.Zero, null,
                    RecycleBinFlags.NoConfirmation | RecycleBinFlags.NoProgressUI | RecycleBinFlags.NoSound), token
                );

                if (result == 0) // Se o resultado for 0, sucesso na exclusão
                {
                    _MainForm.Sucesso++;
                    await _MainForm.Log_MensagemAsync("Lixeira: Apagada!", true);
                    await Task.Delay(500);
                    _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
                }

                if (result == 2147549183) // Lixeira já estava vazia
                {
                    _MainForm.Sucesso++;
                    await _MainForm.Log_MensagemAsync("Lixeira: Já estava vazia", true);
                    await Task.Delay(500);
                    _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
                }         
            }
            catch (Exception e)
            {
                _MainForm.Erro++;
               await _MainForm.Log_MensagemAsync($"Lixeira: Erro ao apagar. Detalhes: \n  {e.Message}", true);
            }
        }
    }
}
