using MeuSuporte;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskScheduler
{
    internal class Class_CleanDirectorry
    {
        string DiretorioPasta = @"";
        int NumFiles = 0;
        int NumFolder = 0;
        FileInfo _Arquivo;
        System.IO.DirectoryInfo _Pasta;
        string _User = "PCs";
        private MainForm _MainForm;
        FileSecurity _FileSecurity = new FileSecurity();
        DirectorySecurity _DirectorySecurity = new DirectorySecurity();
        System.Windows.Forms.Form _FormList;

        public Class_CleanDirectorry(MainForm formPreventiva)
        {
            _MainForm = formPreventiva;
        }

        private async Task DeleteFileAsync(string txt) // deleta o arquivo de forma assíncrona
        {
            if (_MainForm.AbortExecution)
            {
                return;
            }
            try
            {
                _Arquivo = new FileInfo(txt); // atribui o arquivo
                _Arquivo.SetAccessControl(_FileSecurity); // atribui o acesso
                _Arquivo.Delete(); // deleta o arquivo
                NumFiles++;
            }
            catch
            {
                // _formPreventiva.Log_Mensagem("Erro ao Apagar arquivo: ", txt);
            }
        }

        private async Task DeleteFolderAsync(string txt) // deleta a pasta de forma assíncrona
        {
            _Pasta = new System.IO.DirectoryInfo(txt); // atribui a pasta
            _Pasta.SetAccessControl(_DirectorySecurity); // atribui o acesso para a pasta
            try
            {
                _Pasta.Delete(true); // deleta a pasta
                NumFolder++;
            }
            catch
            {
                // _formPreventiva.Log_Mensagem("Erro ao Apagar Pasta: ", txt);
            }
        }

        private async Task<bool> UserSecurityAsync() // atribui as permissões de segurança de forma assíncrona
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
                _MainForm.Erro++;
                retorno = false;
                _MainForm.Log_Mensagem("Erro ao fazer atribuição de Segurança nos Arquivos", " !");
            }
            return retorno;
        }

        private async Task ListFilesAsync() // lista e deleta arquivos e pastas de forma assíncrona
        {
            try
            {
                DirectoryInfo DiretorioRaiz = new DirectoryInfo(DiretorioPasta);
                DirectoryInfo[] Array_Pastas = DiretorioRaiz.GetDirectories();
                FileInfo[] Diret = new DirectoryInfo(DiretorioPasta).GetFiles("*.*", SearchOption.AllDirectories);

                foreach (FileInfo Dir in Diret)
                {
                    await DeleteFileAsync(Dir.FullName); // Exclui os arquivos de forma assíncrona
                }

                foreach (DirectoryInfo Dir in Array_Pastas)
                {
                    await DeleteFolderAsync(Dir.FullName); // Exclui as pastas de forma assíncrona
                }
            }
            catch (Exception e)
            {
                _MainForm.Log_Mensagem("Erro ao listar os Arquivos de SoftwareDistribution", "\r\n" + e.Message);
            }
        }

        public async Task CleanDirectoryAsync(string _DiretorioPasta, string _NameFolder) // Método principal assíncrono
        {
            DiretorioPasta = _DiretorioPasta;
            NumFiles = 0;
            NumFolder = 0;

            _User = Environment.UserName.ToString(); // atribui o nome do usuário

            if (DiretorioPasta != null)
            {
                if (await UserSecurityAsync()) // Usa a versão assíncrona do método UserSecurity
                {
                    await ListFilesAsync(); // Usa a versão assíncrona do método ListFiles
                    _MainForm.Sucesso++;
                    _MainForm.Log_Mensagem("Limpeza da pasta " + _NameFolder + " : " + NumFolder + " Pasta(s) Apagada(s) e " + NumFiles + " Arquivo(s) Apagado(s)", "");
                }
            }
            else
            {
                _MainForm.Erro++;
                _MainForm.Log_Mensagem("Ocorreu um erro ao tentar obter o diretório da pasta", "");
                return;
            }
        }

        void AtribuiUsuario()
        {
            if (MessageBox.Show("Deseja permanecer com o acesso desse Usuário para essa tarefa?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _User = Environment.UserName.ToString();
            }
            else
            {
                _FormList = new FormSelecaoUsuario();
                _FormList.ShowDialog();
                _User = (_FormList as FormSelecaoUsuario).Nome;
            }
        }





    }
}
