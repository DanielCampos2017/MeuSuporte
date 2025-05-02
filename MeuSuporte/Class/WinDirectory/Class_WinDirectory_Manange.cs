using System;
using System.IO;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_WinDirectory_Manange
    {
        private int NumFiles = 0;
        private int NumFolder = 0;
        private MainForm _MainForm;
        private FileSecurity _FileSecurity = new FileSecurity();
        private DirectorySecurity _DirectorySecurity = new DirectorySecurity();



        Class_WinDirectory_ListFiles _WinDirectory_ListFiles;
        Class_WinDirectory_Security _WinDirectory_FileSecurity;
        Class_WinDirectory_Path _WinDirectory_Path;

        public Class_WinDirectory_Manange(MainForm Form_)
        {
            _MainForm = Form_;
            _WinDirectory_ListFiles = new Class_WinDirectory_ListFiles(_MainForm);
            _WinDirectory_FileSecurity = new Class_WinDirectory_Security(_FileSecurity, _DirectorySecurity, Environment.UserName.ToString());
            _WinDirectory_Path = new Class_WinDirectory_Path();
        }
              
        // verifica se o diretorio existe
        private bool checkPath(string Path)
        {
            return Directory.Exists(Path);
        }

 

        public async Task CleanDirectoryAsync(string _DiretorioPasta, string _NameFolder, int ValueUniProgressBar, CancellationToken token) // Método principal assíncrono
        {
            NumFiles = 0;
            NumFolder = 0;

            // Crie o diretorio caso ele nao exista
            if (_WinDirectory_Path.CheckPath(_DiretorioPasta) == false)
            {
                _MainForm.Erro++;
                await _MainForm.Log_MensagemAsync($"Ocorreu um erro ao tentar acessa o diretório {_NameFolder}", true);
                return;
            }

            // atribui as permissões de segurança
            if (await _WinDirectory_FileSecurity.SecurityAsync() == false)
            {
                _MainForm.Erro++;
                await _MainForm.Log_MensagemAsync("Erro ao fazer atribuição de Segurança nos Arquivos", true);
                return;
            }

            // funcao de apagar os arquivos
            await _WinDirectory_ListFiles.Execute(ValueUniProgressBar, _DiretorioPasta, _NameFolder, token);

            await _MainForm.Log_MensagemAsync("\r\n", true);
            await _MainForm.Log_MensagemAsync($"Limpeza da pasta {_NameFolder} : {NumFolder} Pasta(s) Apagada(s) e {NumFiles} Arquivo(s) Apagado(s)", false);
          
        }

    }
}
