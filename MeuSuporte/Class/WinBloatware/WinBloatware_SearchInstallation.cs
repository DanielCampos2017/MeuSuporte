using System.Diagnostics;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBloatware_SearchInstallation
    {
        public async Task<bool> Search(WinBloatware_Format BloatApp)
        {
            bool retorno = false;

            //Get-AppxPackage -AllUsers | Where-Object {$_.Name -like "*Weather*"}
            //Arguments = $"Get-AppxPackage -AllUsers *{BloatApp.Comando}*",
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "powershell",
                Arguments = $"Get-AppxPackage -AllUsers | Where-Object  {{$_.Name -like *{BloatApp.Command}*}}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                // verifica se teve retorno 
                if (!string.IsNullOrWhiteSpace(output))
                {
                    retorno = true;
                }
            }
            return retorno;
        }
    }
}
