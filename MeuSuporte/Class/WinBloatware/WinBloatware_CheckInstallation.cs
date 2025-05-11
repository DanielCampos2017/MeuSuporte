using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBloatware_CheckInstallation
    {
        private readonly WinGlobal_UIService UIService;
        WinBloatware_RenoveNewUser RenoveNewUser;
        WinBloatware_RenoveAllUser RenoveAllUser;
        private readonly WinBloatware_SearchInstallation SearchInstallation;

        public WinBloatware_CheckInstallation(WinGlobal_UIService ui)
        {
            UIService = ui;
            RenoveAllUser = new WinBloatware_RenoveAllUser(ui);
            RenoveNewUser = new WinBloatware_RenoveNewUser(ui);
            SearchInstallation = new WinBloatware_SearchInstallation();
        }

        public async Task Check(WinBloatware_Format BloatApp, CancellationToken token)
        {
            bool isInstalled = await SearchInstallation.Search(BloatApp);

            if (!isInstalled)
            {
                await UIService.Log_MensagemAsync($"{BloatApp.Title} {{{BloatApp.Command}}} - not found", true); // Bloatware não instalado
                return;
            }
            await RenoveAllUser.Renove(BloatApp, token);
            await RenoveNewUser.Renove(BloatApp, token);
        }
    }
}
