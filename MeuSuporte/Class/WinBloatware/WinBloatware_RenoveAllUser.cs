using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBloatware_RenoveAllUser
    {
        private readonly WinGlobal_UIService UIService;
        private readonly WinBloatware_MessageErroList MessageErroList;
        private readonly WinBloatware_SearchInstallation SearchInstallation;

        public WinBloatware_RenoveAllUser(WinGlobal_UIService ui)
        {
            UIService = ui;
            MessageErroList = new WinBloatware_MessageErroList(ui);
            SearchInstallation = new WinBloatware_SearchInstallation();
        }

        public async Task Renove(WinBloatware_Format BloatApp, CancellationToken token)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "powershell",
                Arguments = $"Get-AppxPackage -AllUsers *{BloatApp.Command}* | Remove-AppxPackage -AllUsers",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                //string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                await Task.Delay(500);
                if (!string.IsNullOrWhiteSpace(error))
                {
                    await MessageErroList.MessageError(error, BloatApp.Title, BloatApp.Command);
                    return;
                }

                bool isInstalled = await SearchInstallation.Search(BloatApp);

                if (!isInstalled)
                {
                    await UIService.Log_MensagemAsync($"{BloatApp.Title} {{{BloatApp.Command}}} All User - uninstall success", true);
                }
                else
                {
                    await UIService.Log_MensagemAsync($"{BloatApp.Title} {{{BloatApp.Command}}} All User - uninstall failed", true);
                }
            }
            await Task.Delay(1000);
        }
    }
}
