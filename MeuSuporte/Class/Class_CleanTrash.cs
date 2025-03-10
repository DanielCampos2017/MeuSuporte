using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TaskScheduler
{
    internal class Class_CleanTrash
    {
        private MainForm _MainForm;

        // Importação do método nativo para limpar a lixeira
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, Binary dwFlags);

        [Flags]
        enum Binary
        {
            SHERB_NOCONFIRMATION = 0x00000001, // Sem confirmação
            SHERB_NOPROGRESSUI = 0x00000002,   // Sem barra de progresso
            SHERB_NOSOUND = 0x00000004         // Sem som
        }

        public Class_CleanTrash(MainForm formPreventiva)
        {
            _MainForm = formPreventiva;
        }

        public async Task CleanTrash()
        {
            if (_MainForm.AbortExecution)
            {
                return;
            }
            try
            {
                _MainForm.Log_Mensagem("Lixeira:", "Apagando...");

                // Aguarda a execução para garantir que a lixeira seja esvaziada antes de continuar

                uint result = SHEmptyRecycleBin(IntPtr.Zero, null, Binary.SHERB_NOCONFIRMATION | Binary.SHERB_NOPROGRESSUI | Binary.SHERB_NOSOUND);

                if (result == 0) // Se o resultado for 0, sucesso na exclusão
                {
                    _MainForm.Sucesso++;
                  // _formPreventiva.Log_Mensagem("Lixeira:", "Apagada!");
                }
                else
                {
                    throw new Exception($"Código de erro retornado: {result}");
                }
            }
            catch (Exception e)
            {
                _MainForm.Erro++;
                _MainForm.Log_Mensagem("Lixeira:", "Erro ao apagar. Detalhes: \n" + e.Message);
            }
        }
    }
}
