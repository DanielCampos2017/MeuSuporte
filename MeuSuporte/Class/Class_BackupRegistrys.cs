using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_BackupRegistrys
    {
        private MainForm _MainForm;
        private Class_GeraNomePasta _Class_GeraNomePasta;

        public Class_BackupRegistrys(MainForm Form_)
        {
            _MainForm = Form_;
        }

        public async Task BackupRegistry(string _Directory, string _Name, RegistryKey _Registry, int ValueUniProgressBar)
        {
            _MainForm.ProgressBarADD(ValueUniProgressBar / 2);

            _Class_GeraNomePasta = new Class_GeraNomePasta();
            string diretorio = _Class_GeraNomePasta.Directory();

            diretorio += _Directory;

            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }

            // Inicializa o StringBuilder para armazenar o conteúdo do arquivo .reg
            StringBuilder regFile = new StringBuilder();
            regFile.AppendLine("Windows Registry Editor Version 5.00");
            regFile.AppendLine("");

            // Chama a função recursiva para capturar todas as subchaves e valores
            ProcessSubkey(_Registry, regFile);

            try
            {
                // Escreve o backup no arquivo com codificação Unicode de forma assíncrona
                File.WriteAllText(diretorio + "\\" + _Name, regFile.ToString(), Encoding.Unicode);
                _MainForm.Sucesso++;
                _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
                _MainForm.Log_MensagemAsync($"Backup concluído: Backup salvo em {diretorio}", true);
            }
            catch (Exception e)
            {
                _MainForm.Erro++;
                Task task = _MainForm.Log_MensagemAsync("Erro ao salvar backup: " + e.Message, true);
            }
        }

        private void ProcessSubkey(RegistryKey Key, StringBuilder RegistryFile)
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
                string FormattedValue = FormatRegisterValue(TypeValue, valor);

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
                    ProcessSubkey(subChave, RegistryFile);
                }
            }
        }

        private string FormatRegisterValue(RegistryValueKind KeyType, object KeyValue)
        {
            switch (KeyType)
            {
                case RegistryValueKind.String:
                    // Para valores do tipo string, substituímos "\" por "\\" para manter a formatação correta no arquivo .reg.
                    return $"\"{KeyValue.ToString().Replace("\\", "\\\\")}\"";

                case RegistryValueKind.DWord:
                    // Converte valores inteiros para o formato hexadecimal de 8 dígitos, prefixado por "dword:"
                    return $"dword:{((int)KeyValue):X8}";

                case RegistryValueKind.QWord:
                    // Converte valores longos (64 bits) para hexadecimal de 16 dígitos, prefixado por "qword:"
                    return $"qword:{((long)KeyValue):X16}";

                case RegistryValueKind.ExpandString:
                    // Para ExpandString, retorna como uma string normal sem necessidade de conversão especial.
                    return $"\"{KeyValue}\"";

                case RegistryValueKind.Binary:
                    // Converte um array de bytes em formato hexadecimal separado por vírgulas (exemplo: hex:DE,AD,BE,EF).
                    byte[] bytes = (byte[])KeyValue;
                    return "hex:" + BitConverter.ToString(bytes).Replace("-", ",");

                case RegistryValueKind.MultiString:
                    // Para valores MultiString, converte cada string para bytes Unicode, adiciona um separador nulo (0,0)
                    // entre elas e formata em hexadecimal, conforme o padrão do Windows Registry Editor.
                    string[] valores = (string[])KeyValue;
                    return "hex(7):" + string.Join(",", valores
                        .SelectMany(v => Encoding.Unicode.GetBytes(v).Concat(new byte[] { 0, 0 }))
                        .Select(b => b.ToString("X2")));

                default:
                    // Retorna null caso o tipo de valor não seja reconhecido.
                    return null;
            }
        }
    }
}
