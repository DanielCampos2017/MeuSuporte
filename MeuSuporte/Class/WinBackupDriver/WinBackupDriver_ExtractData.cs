using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBackupDriver_ExtractData
    {
        private readonly WinBackupDriver_ValueUnit ValueUnit;

        public WinBackupDriver_ExtractData()
        {
            ValueUnit = new WinBackupDriver_ValueUnit();
        }

        public async Task DriverBackup_ExtractProgress(string valor)
        {
            // Verifica se a string contém a palavra "Exportando"
            //valor esperado = Exportando 10 de 74 - oem.inf: O pacote de driver foi exportado com sucesso."

            if (!valor.Contains("Exportando"))
            {
                return;
            }

            // Expressão regular para capturar números
            MatchCollection matches = Regex.Matches(valor, @"\d+");

            int valorMaximo = (matches.Count > 1) ? Convert.ToInt32(matches[1].Value) : 100;
            float unidade = (float)WinGlobal_UIService.Instance.ValueUniProgressBar / (float)valorMaximo;

            WinGlobal_UIService.Instance.ProgressBarADD(await ValueUnit.Value(unidade));
        }

    }
}
