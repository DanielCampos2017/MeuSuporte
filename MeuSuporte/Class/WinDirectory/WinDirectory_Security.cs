using System;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinDirectory_Security
    {
        public async Task<bool> SecurityAsync(FileSecurity fileSecurity, DirectorySecurity directorySecurity, string User) // atribui as permissões de segurança
        {
            bool retorno = false;

            try
            {
                directorySecurity.AddAccessRule(new FileSystemAccessRule(User, FileSystemRights.Modify, AccessControlType.Allow));
                directorySecurity.SetAccessRuleProtection(false, false);
                directorySecurity.AddAccessRule(new FileSystemAccessRule(User, FileSystemRights.Modify, AccessControlType.Allow));
                directorySecurity.SetAccessRuleProtection(false, false);
                retorno = true;
            }
            catch (Exception e)
            {
                retorno = false;
            }
            return retorno;
        }

    }
}
