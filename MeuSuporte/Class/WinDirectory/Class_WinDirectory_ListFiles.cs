using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_WinDirectory_ListFiles
    {
        
        private MainForm _MainForm;
        Class_WinDirectory_File _WinDirectory_File;
        Class_WinDirectory_Folder _WinDirectory_Folder;


        public Class_WinDirectory_ListFiles(MainForm Form_)
        {
            _MainForm = Form_;
            _WinDirectory_File = new Class_WinDirectory_File();
            _WinDirectory_Folder = new Class_WinDirectory_Folder();            
        }

        public async Task Execute(int ValueUniProgressBar, string PathFolder, string _NameFolder, CancellationToken token)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(PathFolder);
                var files = directory.EnumerateFiles("*.*", SearchOption.AllDirectories);
                var folders = directory.EnumerateDirectories();
                     
                int ValueUniBar = ValueUniProgressBar;
                int total = directory.EnumerateFiles("*.*", SearchOption.AllDirectories).Count() + folders.Count();// + pastas.Count;
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
                      //  _MainForm.ProgressBarADD(1);
                        valorAcumulado -= 1;
                        await _MainForm.Log_MensagemAsyncSobrescrever($"Apagando arquivos {total} / {loop} da Pasta {{{_NameFolder}}}");
                        await Task.Delay(20);
                    }
                    // await DeleteFileAsync(file.FullName);   // deleta o arquivo
                    await _WinDirectory_File.Delete(file.FullName, token);

                }


                foreach (var folder in folders)
                {
                    token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                    loop++;
                    valorAcumulado += valorUnidade;
                    if (valorAcumulado >= 1)
                    {
                      //  _MainForm.ProgressBarADD(1);
                        valorAcumulado -= 1;
                        await _MainForm.Log_MensagemAsyncSobrescrever($"Apagando arquivos {total} / {loop} da Pasta {{{_NameFolder}}}");
                        await Task.Delay(20);
                    }
                    // await DeleteFolderAsync(pasta.FullName); // deleta a pasta
                    await _WinDirectory_Folder.Delete(folder.FullName, token);
                }

               // _MainForm.Sucesso++;
                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
               // _MainForm.Erro++;
            }
        }
    }
}
