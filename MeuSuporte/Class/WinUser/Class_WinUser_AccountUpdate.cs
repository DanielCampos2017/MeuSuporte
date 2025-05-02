using System.DirectoryServices;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace MeuSuporte
{
    internal class Class_WinUser_AccountUpdate
    {
        private MainForm _MainForm;
        Class_WinUser_GroupAdministrators WinUser_GroupAdministrators;
        Class_WinUser_GroupRemote WinUser_GroupRemote;
        Class_WinUser_State WinUser_State;

        public Class_WinUser_AccountUpdate(MainForm Form_)
        {
            _MainForm = Form_;
            WinUser_GroupAdministrators = new Class_WinUser_GroupAdministrators();
            WinUser_GroupRemote = new Class_WinUser_GroupRemote();
            WinUser_State = new Class_WinUser_State();
        }

        public async Task Update(CancellationToken token, int ValueUniProgressBar, string NameUser, string PasswordUser) // altera a senha do usuario selecionado
        {
            try
            {
                _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
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

                            _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
                            _MainForm.Log_MensagemAsync($"Conta Local de Suporte Tecnico atualizado !", true);
                            _MainForm.Log_MensagemAsync($"Usuario: {NameUser}", true);
                            _MainForm.Log_MensagemAsync($"Senha: {PasswordUser}", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _MainForm.Erro++;
                _MainForm.Log_MensagemAsync($"Erro:  {ex.Message}", true);
            }
        }

    }
}
