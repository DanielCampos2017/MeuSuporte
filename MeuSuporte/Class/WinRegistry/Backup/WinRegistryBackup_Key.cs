using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MeuSuporte
{
    internal class WinRegistryBackup_Key
    {
        private  WinRegistryBackup_RegistryFile RegistryFile;
        private  WinRegistryBackup_ProcessSubkey ProcessSubkey;

        public async Task Backup(string NameFolder, RegistryKey RegistryCurrent, int ValueUniProgressBar)
        {
            RegistryFile = new WinRegistryBackup_RegistryFile();
            ProcessSubkey = new WinRegistryBackup_ProcessSubkey();

            WinGlobal_UIService.Instance.token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

            // monta registro para salvar
            StringBuilder regFile = new StringBuilder();
            regFile.AppendLine("Windows Registry Editor Version 5.00");
            regFile.AppendLine("");
         
            ProcessSubkey.Subkey(RegistryCurrent, regFile);

            // Chama a Função de grava a chave no Disco
            await RegistryFile.Write(NameFolder, regFile, ValueUniProgressBar);
        }
    }
}
