using System.Threading.Tasks;
using Microsoft.Win32;

namespace MeuSuporte
{
    internal class WinRegistryBackup_Mananger
    {
        private  WinRegistryBackup_Key RegistryBackup_Key;

        public async Task Mananger()
        {
            RegistryBackup_Key = new WinRegistryBackup_Key();
            WinGlobal_UIService.Instance.token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

            string NameMachineRun = "LocalMachineRun.reg";
            string NameUserRun = "LocalUserRun.reg";
            string NameWOW6432Node = "WOW6432Node.reg";

            RegistryKey RegistryLocalMachineRun = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            RegistryKey RegistryCurrentUserRun = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            RegistryKey RegistryWOW6432Node = Registry.LocalMachine.OpenSubKey(@"Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Run");

            await RegistryBackup_Key.Backup(NameMachineRun, RegistryLocalMachineRun, WinGlobal_UIService.Instance.ValueUniProgressBar / 3);
            await RegistryBackup_Key.Backup(NameUserRun, RegistryCurrentUserRun, WinGlobal_UIService.Instance.ValueUniProgressBar / 3);
            await RegistryBackup_Key.Backup(NameWOW6432Node, RegistryWOW6432Node, WinGlobal_UIService.Instance.ValueUniProgressBar / 3);
        }
    }
}
