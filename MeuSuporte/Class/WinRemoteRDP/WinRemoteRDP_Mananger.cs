using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace MeuSuporte
{
    internal class WinRemoteRDP_Mananger
    {
        private WinRemoteRDP_Disable RemoteRDP_Disable;
        private WinRemoteRDP_Enable RemoteRDP_Enable;

        public WinRemoteRDP_Mananger(WinGlobal_UIService ui)
        {
            RemoteRDP_Disable = new WinRemoteRDP_Disable(ui);
            RemoteRDP_Enable = new WinRemoteRDP_Enable(ui);
        }

        public async Task Mananger(bool valor, CancellationToken token, int ValueUniProgressBar)
        {
            if (valor)
            {
                await RemoteRDP_Enable.Enable(token, ValueUniProgressBar);
            }
            else
            {
                await RemoteRDP_Disable.Disable(token, ValueUniProgressBar);
            }
        }

    }
}
