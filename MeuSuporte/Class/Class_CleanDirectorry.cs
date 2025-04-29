using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_CleanDirectorry
    {
        string DiretorioPasta = @"";
        private int NumFiles = 0;
        private int NumFolder = 0;
        private FileInfo _Arquivo;
        private DirectoryInfo _Pasta;
        private string _User = "PCs";
        private MainForm _MainForm;
        private FileSecurity _FileSecurity = new FileSecurity();
        private DirectorySecurity _DirectorySecurity = new DirectorySecurity();

        public Class_CleanDirectorry(MainForm Form_)
        {
            _MainForm = Form_;
        }

        #region Funcoes internas

        private async Task DeleteFileAsync(string txt) // deleta o arquivo de forma assíncrona
        {
            try
            {
                _Arquivo = new FileInfo(txt); // atribui o arquivo
                _Arquivo.SetAccessControl(_FileSecurity); // atribui o acesso
                _Arquivo.Delete(); // deleta o arquivo
                NumFiles++;
            }
            catch
            {
                // não vou querer mensagem de erro devido ser centenas de arquivos para apagar então não quero entopir o log com erro
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

        private async Task<bool> UserSecurityAsync() // atribui as permissões de segurança
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
                await _MainForm.Log_MensagemAsync("Erro ao fazer atribuição de Segurança nos Arquivos", true);
            }
            return retorno;
        }

        private async Task ListFilesAsync(int ValueUniProgressBar, string _NameFolder, CancellationToken token)
        {
            try
            {
                DirectoryInfo diretorio = new DirectoryInfo(DiretorioPasta);
                var files = diretorio.EnumerateFiles("*.*", SearchOption.AllDirectories);
                var pastas = diretorio.EnumerateDirectories();

                int ValueUniBar = ValueUniProgressBar;
                int total = diretorio.EnumerateFiles("*.*", SearchOption.AllDirectories).Count() + pastas.Count();// + pastas.Count;
                float valorUnidade = (float)ValueUniBar / total;
                float valorAcumulado = 0f;
                int loop = 0;

                foreach (var file in files)
                {
                    token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                    
                    loop++;
                    valorAcumulado += valorUnidade;
                    if (valorAcumulado >= 1)
                    {
                        _MainForm.ProgressBarADD(1);
                        valorAcumulado -= 1;
                        await _MainForm.Log_MensagemAsyncSobrescrever($"Apagando arquivos {total} / {loop} da Pasta {{{_NameFolder}}}");

                        await Task.Delay(20);
                    }
                    await DeleteFileAsync(file.FullName);   // deleta o arquivo                 
                }
                                

                foreach (var pasta in pastas)
                {
                    token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                    loop++;
                    valorAcumulado += valorUnidade;
                    if (valorAcumulado >= 1)
                    {
                        _MainForm.ProgressBarADD(1);
                        valorAcumulado -= 1;
                        await _MainForm.Log_MensagemAsyncSobrescrever($"Apagando arquivos {total} / {loop} da Pasta {{{_NameFolder}}}");
                        await Task.Delay(20);
                    }
                    await DeleteFolderAsync(pasta.FullName); // deleta a pasta
                }

                _MainForm.Sucesso++;
                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                _MainForm.Erro++;
            }
        }

        // verifica se o diretorio existe
        private bool checkPath(string Path)
        {
            return Directory.Exists(Path);
        }

        #endregion

     
        #region Tarefas Plublicas

        public async Task CleanDirectoryAsync(string _DiretorioPasta, string _NameFolder, int ValueUniProgressBar, CancellationToken token) // Método principal assíncrono
        {
            DiretorioPasta = _DiretorioPasta;
            NumFiles = 0;
            NumFolder = 0;
            _User = Environment.UserName.ToString(); // atribui o nome do usuário

            // verifica se o diretorio existe
            if (checkPath(DiretorioPasta) == false)
            {
                _MainForm.Erro++;
                await _MainForm.Log_MensagemAsync($"Ocorreu um erro ao tentar acessa o diretório {_NameFolder}", true);
                return;
            }
              
            // atribui as permissões de segurança
            if (await UserSecurityAsync() == false)
            {
                _MainForm.Erro++;
                await _MainForm.Log_MensagemAsync("Erro ao fazer atribuição de Segurança nos Arquivos", true);
            }

            // funcao de apagar os arquivos
            await ListFilesAsync(ValueUniProgressBar, _NameFolder, token);

            await _MainForm.Log_MensagemAsync("\r\n", true);
            await _MainForm.Log_MensagemAsync($"Limpeza da pasta {_NameFolder} : {NumFolder} Pasta(s) Apagada(s) e {NumFiles} Arquivo(s) Apagado(s)", false);
          
        }

        #endregion

    }
}
