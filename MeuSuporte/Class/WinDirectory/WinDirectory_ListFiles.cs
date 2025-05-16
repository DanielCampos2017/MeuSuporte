using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinDirectory_ListFiles
    {
        private WinDirectory_File _WinDirectory_File;
        private WinDirectory_Folder _WinDirectory_Folder;

        public WinDirectory_ListFiles()
        {
            _WinDirectory_File = new WinDirectory_File();
            _WinDirectory_Folder = new WinDirectory_Folder();            
        }

        public async Task Remove(int ValueUniProgressBar, string PathFolder, string _NameFolder, CancellationToken token)
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
                        WinGlobal_UIService2.Instance.ProgressBarADD(1);
                        valorAcumulado -= 1;
                        await WinGlobal_UIService2.Instance.Log_MensagemAsyncSobrescrever($"Apagando arquivos {total} / {loop} da Pasta {{{_NameFolder}}}");
                        await Task.Delay(20);
                    }
                    await _WinDirectory_File.Delete(file.FullName, token); // deleta o arquivo
                }


                foreach (var folder in folders)
                {
                    token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                    loop++;
                    valorAcumulado += valorUnidade;
                    if (valorAcumulado >= 1)
                    {
                        WinGlobal_UIService2.Instance.ProgressBarADD(1);
                        valorAcumulado -= 1;
                        await WinGlobal_UIService2.Instance.Log_MensagemAsyncSobrescrever($"Apagando arquivos {total} / {loop} da Pasta {{{_NameFolder}}}");
                        await Task.Delay(20);
                    }
                    await _WinDirectory_Folder.Delete(folder.FullName, token); // deleta a pasta
                }
                                
                WinGlobal_UIService2.Instance.Sucesso++;
                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                WinGlobal_UIService2.Instance.Erro++;
            }
        }
    }
}
