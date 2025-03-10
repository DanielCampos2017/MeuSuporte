using System;
using System.Collections;
using System.DirectoryServices;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TaskScheduler
{

    internal class Class_UpdateUser
    {
        private string NameUser;
        private string PasswordUser;
        private MainForm _MainForm;

        public Class_UpdateUser(MainForm formPreventiva)
        {
            _MainForm = formPreventiva;
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

        private async Task<bool> IsUserTerravistaAsync()
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

        private async Task CreateUserTerravistaAsync()
        {
            try
            {
                // cria usuario
                DirectoryEntry EntradaDiretorioGrupos = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
                DirectoryEntry EntradaDiretorioUsuarios = EntradaDiretorioGrupos.Children.Add(NameUser, "user");
                EntradaDiretorioUsuarios.Invoke("SetPassword", new object[] { PasswordUser });
                EntradaDiretorioUsuarios.CommitChanges();

                await AddGroupRemotoAsync(EntradaDiretorioUsuarios);
                await AddGroupAdministratorAsync(EntradaDiretorioUsuarios);

                _MainForm.Sucesso++;
                _MainForm.Log_Mensagem("Usuario " + NameUser + " criado !", "  ");
            }
            catch (Exception ex)
            {
                _MainForm.Erro++;
                _MainForm.Log_Mensagem("Erro: ", ex.Message);
            }
        }

        private async Task UpdateUserAsync() // altera a senha do usuario selecionado
        {
            try
            {
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
                            _MainForm.Log_Mensagem("Usuario terravista atualizado !", "  ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _MainForm.Erro++;
                _MainForm.Log_Mensagem("Erro: ", ex.Message);
            }
        }


        public async Task Usuario()
        {
            Class_Password _ClassPassword = new Class_Password();
            NameUser = _ClassPassword.GetUser();
            PasswordUser = _ClassPassword.GetPassword();

            if (_MainForm.AbortExecution)
            {
                return;
            }

            if (!await IsUserTerravistaAsync())
            {
                // se não existir
                await CreateUserTerravistaAsync();
            }
            else
            {
                // se existir
                await UpdateUserAsync();
            }
        }
    }
}
