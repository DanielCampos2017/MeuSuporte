using System.Diagnostics;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBackupDriver_ProcessInfo
    {
        public async Task<ProcessStartInfo> Create(string diretorio)
        {
            string argumento = $"dism /online /export-driver /destination:\"{diretorio}\"";
            return new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C {argumento}",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Verb = "runas"
            };
        }
    }
}
