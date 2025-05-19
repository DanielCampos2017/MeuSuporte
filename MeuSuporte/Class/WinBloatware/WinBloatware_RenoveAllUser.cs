using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBloatware_RenoveAllUser
    {
        private  WinBloatware_MessageErroList MessageErroList;
        private  WinBloatware_SearchInstallation SearchInstallation;

        public async Task Renove(WinBloatware_Format BloatApp)
        {
            MessageErroList = new WinBloatware_MessageErroList();
            SearchInstallation = new WinBloatware_SearchInstallation();

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
                   // await WinGlobal_UIService.Instance.Log_MensagemAsync($"{BloatApp.Title} {{{BloatApp.Command}}} All User - uninstall success", true);
                }
                else
                {
                   // await WinGlobal_UIService.Instance.Log_MensagemAsync($"{BloatApp.Title} {{{BloatApp.Command}}} All User - uninstall failed", true);
                }
            }
            await Task.Delay(1000);
        }
    }
}
