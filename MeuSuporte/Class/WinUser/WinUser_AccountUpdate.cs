using System.DirectoryServices;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace MeuSuporte
{
    internal class WinUser_AccountUpdate
    {
        private readonly WinGlobal_UIService UIService;
        WinUser_GroupAdministrators WinUser_GroupAdministrators;
        WinUser_GroupRemote WinUser_GroupRemote;
        WinUser_State WinUser_State;

        public WinUser_AccountUpdate(WinGlobal_UIService ui)
        {
            UIService = ui;
            WinUser_GroupAdministrators = new WinUser_GroupAdministrators();
            WinUser_GroupRemote = new WinUser_GroupRemote();
            WinUser_State = new WinUser_State();
        }

        public async Task Update(CancellationToken token, int ValueUniProgressBar, string NameUser, string PasswordUser) // altera a senha do usuario selecionado
        {
            try
            {
                UIService.ProgressBarADD(ValueUniProgressBar / 2);
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                DirectoryEntry EntradaUsuario = new DirectoryEntry("WinNT://" + Environment.MachineName + ",Computer");

                if (EntradaUsuario.Children != null)
                {
                    var results = EntradaUsuario.Children.Cast<DirectoryEntry>().Where(r => r.SchemaClassName == "User").OrderBy(r => r.Name);

                    foreach (DirectoryEntry child in results)
                    {
                        if (child.Name == NameUser) // seleciona o Usuario
                        {
                            await WinUser_GroupAdministrators.Add(child);  // adiciona no grupo
                            await WinUser_GroupRemote.Add(child);  // adiciona no grupo

                            await WinUser_State.State(child, false); // abilita usuario
                            child.Invoke("SetPassword", new Object[] { PasswordUser });   // altera a senha do usuario

                            UIService.ProgressBarADD(ValueUniProgressBar / 2);
                            UIService.Log_MensagemAsync($"Conta Local de Suporte Tecnico atualizado !", true);
                            UIService.Log_MensagemAsync($"Usuario: {NameUser}", true);
                            UIService.Log_MensagemAsync($"Senha: {PasswordUser}", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UIService.Erro++;
                UIService.Log_MensagemAsync($"Erro:  {ex.Message}", true);
            }
        }

    }
}
