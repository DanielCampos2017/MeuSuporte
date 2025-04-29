using System;
using System.Collections;
using System.DirectoryServices;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{

    internal class Class_UpdateUser
    {
        private string NameUser;
        private string PasswordUser;
        private MainForm _MainForm;

        public Class_UpdateUser(MainForm Form_)
        {
            _MainForm = Form_;
        }

        private async Task EnableUserAsync(DirectoryEntry de, bool tipo)
        {
            // true == conta desabilitada
            // false == conta abilitada
            object ent = de.NativeObject;
            Type type = ent.GetType();
            type.InvokeMember("AccountDisabled", BindingFlags.SetProperty, null, ent, new object[] { tipo });
            de.CommitChanges();
            await Task.CompletedTask;
        }

        private async Task<bool> IsUserAsync()
        {

            DirectoryEntry machine = new DirectoryEntry("WinNT://" + Environment.MachineName + ",Computer");

            if (machine.Children != null)
            {
                var results = machine.Children.Cast<DirectoryEntry>().Where(r => r.SchemaClassName == "User").OrderBy(r => r.Name);
                foreach (DirectoryEntry child in results)
                {
                    if (child.Name != "WDAGUtilityAccount" && child.Name != "DefaultAccount" && child.Name == NameUser)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private async Task AddGroupRemotoAsync(DirectoryEntry _User)
        {
            // Adiciona no grupo acesso remoto
            DirectoryEntry Group = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
            DirectoryEntry GroupRemote = Group.Children.Find("Usuários da área de trabalho remota", "group");

            if (GroupRemote != null)
            {
                // Verifica se o usuário já faz parte do grupo
                foreach (object membro in (IEnumerable)GroupRemote.Invoke("Members"))
                {
                    using (DirectoryEntry membroEntry = new DirectoryEntry(membro))
                    {
                        if (membroEntry.Path == _User.Path)
                        {
                            return; // Sai da função, pois o usuário já está no grupo
                        }
                    }
                }
                GroupRemote.Invoke("Add", new object[] { _User.Path });    // adiciona no grupo
            }
            await Task.CompletedTask;
        }

        private async Task AddGroupAdministratorAsync(DirectoryEntry _User)
        {
            // Obtém o grupo de administradores
            DirectoryEntry Grupo = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
            DirectoryEntry GroupAdministrators = Grupo.Children.Find("Administradores", "group");

            if (GroupAdministrators != null)
            {
                // Verifica se o usuário já faz parte do grupo
                foreach (object membro in (IEnumerable)GroupAdministrators.Invoke("Members"))
                {
                    using (DirectoryEntry membroEntry = new DirectoryEntry(membro))
                    {
                        if (membroEntry.Path == _User.Path)
                        {
                            return; // Sai da função, pois o usuário já está no grupo
                        }
                    }
                }
                GroupAdministrators.Invoke("Add", new object[] { _User.Path }); // adiciona no grupo
            }
            await Task.CompletedTask;
        }

        private async Task CreateUserAsync(CancellationToken token, int ValueUniProgressBar)
        {
            try
            {
                _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                // cria usuario
                DirectoryEntry EntradaDiretorioGrupos = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
                DirectoryEntry EntradaDiretorioUsuarios = EntradaDiretorioGrupos.Children.Add(NameUser, "user");
                EntradaDiretorioUsuarios.Invoke("SetPassword", new object[] { PasswordUser });
                EntradaDiretorioUsuarios.CommitChanges();

                await AddGroupRemotoAsync(EntradaDiretorioUsuarios);
                await AddGroupAdministratorAsync(EntradaDiretorioUsuarios);

                _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
                _MainForm.Sucesso++;
                _MainForm.Log_MensagemAsync($"Conta Local de Suporte Tecnico criado !", true);
                _MainForm.Log_MensagemAsync($"Usuario: {NameUser}", true);
                _MainForm.Log_MensagemAsync($"Senha {PasswordUser}", true);
            }
            catch (Exception ex)
            {
                _MainForm.Erro++;
                _MainForm.Log_MensagemAsync($"Erro: {ex.Message}", true);
            }
        }

        private async Task UpdateUserAsync(CancellationToken token, int ValueUniProgressBar) // altera a senha do usuario selecionado
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
                            await AddGroupAdministratorAsync(child);  // adiciona no grupo
                            await AddGroupRemotoAsync(child);  // adiciona no grupo

                            await EnableUserAsync(child, false); // abilita usuario
                            child.Invoke("SetPassword", new Object[] { PasswordUser });   // altera a senha do usuario

                            _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
                            _MainForm.Log_MensagemAsync($"Conta Local de Suporte Tecnico atualizado !", true);
                            _MainForm.Log_MensagemAsync($"Usuario: {NameUser}", true);
                            _MainForm.Log_MensagemAsync($"Senha {PasswordUser}", true);
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


        public async Task Usuario(CancellationToken token, int ValueUniProgressBar)
        {
           // Class_Password_Private _ClassPassword = new Class_Password_Private();
            Class_Password_Public _ClassPassword = new Class_Password_Public();
            NameUser = _ClassPassword.GetUser();
            PasswordUser = _ClassPassword.GetPassword();

            if (!await IsUserAsync())
            {
                // se não existir
                await CreateUserAsync(token, ValueUniProgressBar);
            }
            else
            {
                // se existir
                await UpdateUserAsync(token, ValueUniProgressBar);
            }
        }
    }
}
