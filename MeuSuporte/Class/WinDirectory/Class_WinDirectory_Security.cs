using System;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_WinDirectory_Security
    {
        private FileSecurity _FileSecurity;   
        private DirectorySecurity _DirectorySecurity;
        private string _User = "PCs";
     
        public Class_WinDirectory_Security(FileSecurity fileSecurity, DirectorySecurity directorySecurity, string User)
        {
            _FileSecurity = fileSecurity;
            _DirectorySecurity = directorySecurity;
            _User = User;
        }

        public async Task<bool> SecurityAsync() // atribui as permissões de segurança
        {
            bool retorno = false;

            try
            {
                _DirectorySecurity.AddAccessRule(new FileSystemAccessRule(_User, FileSystemRights.Modify, AccessControlType.Allow));
                _DirectorySecurity.SetAccessRuleProtection(false, false);
                _FileSecurity.AddAccessRule(new FileSystemAccessRule(_User, FileSystemRights.Modify, AccessControlType.Allow));
                _FileSecurity.SetAccessRuleProtection(false, false);
                retorno = true;
            }
            catch (Exception e)
            {
                retorno = false;
              //  await _MainForm.Log_MensagemAsync("Erro ao fazer atribuição de Segurança nos Arquivos", true);
            }
            return retorno;
        }

    }
}
