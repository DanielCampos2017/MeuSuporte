using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_RemoveBloatware
    {
        private MainForm _MainForm;

        public Class_RemoveBloatware(MainForm Form_)
        {
            _MainForm = Form_;
        }

        // Define o struct
        public struct Bloatware
        {
            public string Titulo { get; set; }
            public string Comando { get; set; }
        }

        public async Task Remover(CancellationToken token)
        {
            Class_ListBloatware _ListBloatware = new Class_ListBloatware();
            Bloatware[] ListBloatware = _ListBloatware.List;

            int qtd = 1;

            foreach (var BloatApp in ListBloatware)
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                
                await CheckInstallationAsync(BloatApp, token);  // Executa o a função Async em uma nova thread
                await _MainForm.Bloatware_ExtractProgress(ListBloatware.Length);
            }            
        }

        async Task CheckInstallationAsync(Bloatware BloatApp, CancellationToken token)
        {
            bool isInstalled = await SearchInstallation(BloatApp);

            if (!isInstalled)
            {
                await _MainForm.Log_MensagemAsync($"{BloatApp.Titulo} {{{BloatApp.Comando}}} - not found", true); // Bloatware não instalado
                return;
            }

            await RenoveAllUser(BloatApp, token);
            await RenoveNewUser(BloatApp, token);

        }

        async Task RenoveNewUser(Bloatware BloatApp, CancellationToken token)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "powershell",
                Arguments = $"Get-AppxProvisionedPackage -Online | Where-Object DisplayName -like '*{BloatApp.Comando}*' | Remove-AppxProvisionedPackage -Online",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                await Task.Delay(500);

                if (!string.IsNullOrWhiteSpace(error))
                {
                    await MessageError(error, BloatApp.Titulo, BloatApp.Comando);
                    return;
                }

                bool isInstalled = await SearchInstallation(BloatApp);
                if (!isInstalled)
                {
                    await _MainForm.Log_MensagemAsync($"{BloatApp.Titulo} {{{BloatApp.Comando}}} New User - uninstall success", true);
                }
                else
                {
                    await _MainForm.Log_MensagemAsync($"{BloatApp.Titulo} {{{BloatApp.Comando}}} New User - uninstall failed", true);
                }
            }
            await Task.Delay(1000);
        }

        async Task RenoveAllUser(Bloatware BloatApp, CancellationToken token)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "powershell",
                Arguments = $"Get-AppxPackage -AllUsers *{BloatApp.Comando}* | Remove-AppxPackage -AllUsers",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                //string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                await Task.Delay(500);
                if (!string.IsNullOrWhiteSpace(error))
                {
                    await MessageError(error, BloatApp.Titulo, BloatApp.Comando);
                    return;
                }

                bool isInstalled = await SearchInstallation(BloatApp);

                if (!isInstalled)
                {
                    await _MainForm.Log_MensagemAsync($"{BloatApp.Titulo} {{{BloatApp.Comando}}} All User - uninstall success", true);
                }
                else
                {
                    await _MainForm.Log_MensagemAsync($"{BloatApp.Titulo} {{{BloatApp.Comando}}} All User - uninstall failed", true);
                }
            }
            await Task.Delay(1000);
        }
        

        public async Task MessageError(string outputError, string AppTitulo, string AppComando)
        {
            // Dicionário de erros conhecidos com suas mensagens explicativas
            Dictionary<string, string> errosConhecidos = new Dictionary<string, string>
            {
                {
                    "0x80073CFA",
                    "Este aplicativo faz parte do Windows e não pode ser desinstalado no nível do usuário.\n" +
                    "Um administrador pode tentar remover o aplicativo do computador usando a opção \"Ativar e desativar recursos do Windows\".\n" +
                    "No entanto, talvez não seja possível desinstalar o aplicativo.\n"
                },
                {
                    "0x80073D01",
                    "O aplicativo está em uso e não pode ser removido ou atualizado neste momento.\n" +
                    "Tente fechar o app ou reiniciar o computador.\n"
                },
                {
                    "0x80073D02",
                    "O aplicativo possui arquivos que estão em uso por outro processo.\n" +
                    "Reinicie o sistema ou feche o aplicativo relacionado.\n"
                },
                {
                    "0x80070032",
                    "A função solicitada não está implementada.\n" +
                    "Pode ocorrer quando se tenta uma operação inválida para o estado do app.\n"
                },
                {
                    "0x80073CF0",
                    "O pacote está danificado ou ausente.\n" +
                    "Geralmente causado por arquivos corrompidos.\n"
                },
                {
                    "0x80073CF3",
                    "Conflito de dependências.\n" +
                    "Pode haver pacotes requeridos que não estão presentes ou não são compatíveis.\n"
                },
                {
                    "0x80073CF6",
                    "Falha geral durante a instalação ou remoção.\n" +
                    "Muitas vezes associada a problemas no manifest ou nas permissões do sistema.\n"
                },
                {
                    "0x80073CFF",
                    "Você está tentando operar sobre um pacote que não existe ou foi removido parcialmente\n"
                },
                {
                    "0x80070005",
                    "Acesso Negado! Você provavelmente não tem permissão.\n" +
                    "Tente executar como administrador pode resolver.\n"
                },
                {
                    "0x80070002",
                    "Arquivo não encontrado.\n" +
                    "Geralmente ocorre quando o caminho de instalação está corrompido ou faltando.\n"
                }
            };

            foreach (var erro in errosConhecidos)
            {
                if (outputError.Contains(erro.Key))
                {
                    await _MainForm.Log_MensagemAsync($"{AppTitulo} {{{AppComando}}} - uninstall failed \nCodigo Erro: {erro.Key} \nMensagem: {erro.Value}  ", true);
                    break; 
                }
            }
        }


        private async Task<bool> SearchInstallation(Bloatware BloatApp)
        {
            bool retorno = false;

            //Get-AppxPackage -AllUsers | Where-Object {$_.Name -like "*Weather*"}
            //Arguments = $"Get-AppxPackage -AllUsers *{BloatApp.Comando}*",
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "powershell",
                Arguments = $"Get-AppxPackage -AllUsers | Where-Object  {{$_.Name -like *{BloatApp.Comando}*}}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                // verifica se teve retorno 
                if (!string.IsNullOrWhiteSpace(output))
                {
                    retorno = true;
                }               
            }
            return retorno;
        }




        //#removido com sucesso
        //Notícia {BingNews} - uninstall success

        //#falha na desinstalação
        //Notícia {BingNews} - uninstall failed

        //#não encontrado
        //Notícia {BingNews} - not found
    }
}
