using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinRemoteRDP_Mananger
    {
        private WinRemoteRDP_Disable RemoteRDP_Disable;
        private WinRemoteRDP_Enable RemoteRDP_Enable;

        public WinRemoteRDP_Mananger()
        {
            RemoteRDP_Disable = new WinRemoteRDP_Disable();
            RemoteRDP_Enable = new WinRemoteRDP_Enable();
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
