using System.Diagnostics;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBackupBCD_ProcessInfo
    {
        public async Task<ProcessStartInfo> Create(string diretorio)
        {
            string arguments = "bcdedit /export " + '"' + diretorio + "\\BCD_Backup.bcd" + '"';
            return new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C {arguments}",
                CreateNoWindow = true,
                UseShellExecute = false,
                Verb = "runas"
            };
        }
    }
}
