using System.DirectoryServices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinUser_AccountCreate
    {
        private readonly WinGlobal_UIService UIService;
        WinUser_GroupAdministrators WinUser_GroupAdministrators;
        WinUser_GroupRemote WinUser_GroupRemote;
       
        public WinUser_AccountCreate(WinGlobal_UIService ui)
        {
            UIService = ui;
            WinUser_GroupAdministrators = new WinUser_GroupAdministrators();
            WinUser_GroupRemote = new WinUser_GroupRemote();
        }

        public async Task Create(CancellationToken token, int ValueUniProgressBar, string NameUser, string PasswordUser )
        {
            try
            {
                UIService.ProgressBarADD(ValueUniProgressBar / 2);
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                // cria usuario
                DirectoryEntry EntradaDiretorioGrupos = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
                DirectoryEntry EntradaDiretorioUsuarios = EntradaDiretorioGrupos.Children.Add(NameUser, "user");
                EntradaDiretorioUsuarios.Invoke("SetPassword", new object[] { PasswordUser });
                EntradaDiretorioUsuarios.CommitChanges();

                // atribui os grupos para o usuario
                await WinUser_GroupRemote.Add(EntradaDiretorioUsuarios);
                await WinUser_GroupAdministrators.Add(EntradaDiretorioUsuarios);

                UIService.ProgressBarADD(ValueUniProgressBar / 2);
                UIService.Sucesso++;
                UIService.Log_MensagemAsync($"Conta Local de Suporte Tecnico criado !", true);
                UIService.Log_MensagemAsync($"Usuario: {NameUser}", true);
                UIService.Log_MensagemAsync($"Senha: {PasswordUser}", true);
            }
            catch (Exception ex)
            {
                UIService.Erro++;
                UIService.Log_MensagemAsync($"Erro: {ex.Message}", true);
            }
        }
    }
}
