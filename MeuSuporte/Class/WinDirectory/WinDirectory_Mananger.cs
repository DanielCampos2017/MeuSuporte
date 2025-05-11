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
        private readonly WinGlobal_UIService UIService;
        private FileSecurity _FileSecurity = new FileSecurity();
        private DirectorySecurity _DirectorySecurity = new DirectorySecurity();

        WinDirectory_ListFiles ListFiles;
        WinDirectory_Security WinDirectory_FileSecurity;
        WinGlobal_DirectoryMananger DirectoryManange;

        public WinDirectory_Mananger(WinGlobal_UIService ui)
        {
            UIService = ui;
            ListFiles = new WinDirectory_ListFiles(UIService);
            WinDirectory_FileSecurity = new WinDirectory_Security(_FileSecurity, _DirectorySecurity, Environment.UserName.ToString());
            DirectoryManange = new WinGlobal_DirectoryMananger();
        }
           
        public async Task Mananger(string DirectoryFolder, string _NameFolder, int ValueUniProgressBar, CancellationToken token) // Método principal assíncrono
        {
            NumFiles = 0;
            NumFolder = 0;

            // verifica se diretorio existe
            if (!DirectoryManange.Check(DirectoryFolder))
            {
                UIService.Erro++;
                await UIService.Log_MensagemAsync($"Ocorreu um erro ao tentar acessa o diretório {_NameFolder}", true);
                return;
            }

            // atribui as permissões de segurança
            if (await WinDirectory_FileSecurity.SecurityAsync() == false)
            {
                UIService.Erro++;
                await UIService.Log_MensagemAsync("Erro ao fazer atribuição de Segurança nos Arquivos", true);
                return;
            }

            // funcao de apagar os arquivos
            await ListFiles.Remove(ValueUniProgressBar, DirectoryFolder, _NameFolder, token);
            await UIService.Log_MensagemAsync("\r\n", true);
            await UIService.Log_MensagemAsync($"Limpeza da pasta {_NameFolder} : {NumFolder} Pasta(s) Apagada(s) e {NumFiles} Arquivo(s) Apagado(s)", false);          
        }

    }
}
