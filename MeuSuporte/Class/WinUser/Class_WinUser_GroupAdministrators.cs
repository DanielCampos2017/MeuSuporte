using System;
using System.Collections;
using System.DirectoryServices;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_WinUser_GroupAdministrators
    {
        public async Task Add(DirectoryEntry _User)
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
        }
    }
}
