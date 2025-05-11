using System.Threading.Tasks;
using Microsoft.Win32;

namespace MeuSuporte
{
    internal class WinRemoteRDP_Registry
    {
        public async Task Enable()
        {
            HabilitarConexaoRemota();
            DesativarAutenticacaoUsuario();
        }

        private void HabilitarConexaoRemota()
        {
            using (RegistryKey chave = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server", true))
            {
                if (chave != null)
                {
                    chave.SetValue("fDenyTSConnections", 0, RegistryValueKind.DWord);
                }
            }
        }

        private void DesativarAutenticacaoUsuario()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", true))
            {
                if (key != null)
                {
                    key.SetValue("UserAuthentication", 0, RegistryValueKind.DWord);
                }
            }
        }
    }
}
