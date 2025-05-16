using System.DirectoryServices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinUser_AccountCreate
    {

        private WinUser_GroupAdministrators WinUser_GroupAdministrators;
        private WinUser_GroupRemote WinUser_GroupRemote;
       
        public WinUser_AccountCreate()
        {
            WinUser_GroupAdministrators = new WinUser_GroupAdministrators();
            WinUser_GroupRemote = new WinUser_GroupRemote();
        }

        public async Task Create(CancellationToken token, int ValueUniProgressBar, string NameUser, string PasswordUser )
        {
            try
            {
                WinGlobal_UIService2.Instance.ProgressBarADD(ValueUniProgressBar / 2);
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                // cria usuario
                DirectoryEntry EntradaDiretorioGrupos = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
                DirectoryEntry EntradaDiretorioUsuarios = EntradaDiretorioGrupos.Children.Add(NameUser, "user");
                EntradaDiretorioUsuarios.Invoke("SetPassword", new object[] { PasswordUser });
                EntradaDiretorioUsuarios.CommitChanges();

                // atribui os grupos para o usuario
                await WinUser_GroupRemote.Add(EntradaDiretorioUsuarios);
                await WinUser_GroupAdministrators.Add(EntradaDiretorioUsuarios);

                WinGlobal_UIService2.Instance.ProgressBarADD(ValueUniProgressBar / 2);
                WinGlobal_UIService2.Instance.Sucesso++;
                WinGlobal_UIService2.Instance.Log_MensagemAsync($"Conta Local de Suporte Tecnico criado !", true);
                WinGlobal_UIService2.Instance.Log_MensagemAsync($"Usuario: {NameUser}", true);
                WinGlobal_UIService2.Instance.Log_MensagemAsync($"Senha: {PasswordUser}", true);
            }
            catch (Exception ex)
            {
                WinGlobal_UIService2.Instance.Erro++;
                WinGlobal_UIService2.Instance.Log_MensagemAsync($"Erro: {ex.Message}", true);
            }
        }
    }
}
