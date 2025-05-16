using System;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinDirectory_Mananger
    {
        private int NumFiles = 0;
        private int NumFolder = 0;
        private FileSecurity _FileSecurity = new FileSecurity();
        private DirectorySecurity _DirectorySecurity = new DirectorySecurity();

        private WinDirectory_ListFiles ListFiles;
        private WinDirectory_Security WinDirectory_FileSecurity;
        private WinGlobal_DirectoryMananger DirectoryManange;

        public WinDirectory_Mananger()
        {
            ListFiles = new WinDirectory_ListFiles();
            WinDirectory_FileSecurity = new WinDirectory_Security();
            DirectoryManange = new WinGlobal_DirectoryMananger();
        }
          
        public async Task Mananger(string DirectoryFolder, string _NameFolder, int ValueUniProgressBar, CancellationToken token) // Método principal assíncrono
        {
            NumFiles = 0;
            NumFolder = 0;
             
            // verifica se diretorio existe
            if (!DirectoryManange.Check(DirectoryFolder))
            {
                WinGlobal_UIService2.Instance.Erro++;    
                await WinGlobal_UIService2.Instance.Log_MensagemAsync($"Ocorreu um erro ao tentar acessa o diretório {_NameFolder}", true);
                return;
            }

            // atribui as permissões de segurança
            if (await WinDirectory_FileSecurity.SecurityAsync(_FileSecurity, _DirectorySecurity, Environment.UserName.ToString()) == false)
            {
                WinGlobal_UIService2.Instance.Erro++;
                await WinGlobal_UIService2.Instance.Log_MensagemAsync("Erro ao fazer atribuição de Segurança nos Arquivos", true);
                return;
            }

            // funcao de apagar os arquivos
            await ListFiles.Remove(ValueUniProgressBar, DirectoryFolder, _NameFolder, token);          
            await WinGlobal_UIService2.Instance.Log_MensagemAsync("\r\n", true);                   
            await WinGlobal_UIService2.Instance.Log_MensagemAsync($"Limpeza da pasta {_NameFolder} : {NumFolder} Pasta(s) Apagada(s) e {NumFiles} Arquivo(s) Apagado(s)", false);          
        }

    }
}
