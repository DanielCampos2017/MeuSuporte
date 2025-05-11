using System.DirectoryServices;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace MeuSuporte
{
    internal class WinUser_Account
    {
        // verifica se existe o usuario
        public async Task<bool> IsEnabled(string NameUser)
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
    }
}
