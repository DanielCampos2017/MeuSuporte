using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MeuSuporte
{
    internal class WinRegistryBackup_ProcessSubkey
    {
        private readonly WinRegistryBackup_FormatRegister FormatRegister;

        public WinRegistryBackup_ProcessSubkey()
        {
            FormatRegister = new WinRegistryBackup_FormatRegister();
        }
        public async Task Subkey(RegistryKey Key, StringBuilder RegistryFile)
        {
            if (Key == null) return;

            // Adiciona a chave atual ao arquivo de backup
            RegistryFile.AppendLine("");
            RegistryFile.AppendLine($"[{Key.Name}]");

            // Processa todos os valores dentro da chave atual
            foreach (string NameValue in Key.GetValueNames())
            {
                object valor = Key.GetValue(NameValue);
                RegistryValueKind TypeValue = Key.GetValueKind(NameValue);

                // Formatar o valor conforme o tipo de dado
                string FormattedValue = await FormatRegister.Format(TypeValue, valor);

                if (FormattedValue != null)
                {
                    RegistryFile.AppendLine($"\"{NameValue}\"={FormattedValue}");
                }
            }

            // Processa recursivamente todas as subchaves
            foreach (string nomeSubchave in Key.GetSubKeyNames())
            {
                using (RegistryKey subChave = Key.OpenSubKey(nomeSubchave))
                {
                    Subkey(subChave, RegistryFile);
                }
            }
        }        
    }
}
