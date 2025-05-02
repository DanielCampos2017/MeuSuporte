using System;
using System.Collections;
using System.DirectoryServices;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_WinUser_GroupRemote
    {
        public async Task Add(DirectoryEntry _User)
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
        }
    }
}
