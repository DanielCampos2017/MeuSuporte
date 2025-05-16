using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBloatware_CheckInstallation
    {
        private WinBloatware_RenoveNewUser RenoveNewUser;
        private WinBloatware_RenoveAllUser RenoveAllUser;
        private readonly WinBloatware_SearchInstallation SearchInstallation;

        public WinBloatware_CheckInstallation()
        {
            RenoveAllUser = new WinBloatware_RenoveAllUser();
            RenoveNewUser = new WinBloatware_RenoveNewUser();
            SearchInstallation = new WinBloatware_SearchInstallation();
        }

        public async Task Check(WinBloatware_Format BloatApp, CancellationToken token)
        {
            bool isInstalled = await SearchInstallation.Search(BloatApp);

            if (!isInstalled)
            {
                await WinGlobal_UIService2.Instance.Log_MensagemAsync($"{BloatApp.Title} {{{BloatApp.Command}}} - not found", true); // Bloatware não instalado
                return;
            }
            await RenoveAllUser.Renove(BloatApp, token);
            await RenoveNewUser.Renove(BloatApp, token);
        }
    }
}
