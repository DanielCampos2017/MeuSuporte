using Microsoft.Win32;
using MeuSuporte.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;





namespace TaskScheduler
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        int ValueUniProgressBar = 0;
        int qtdMarcados = 0;
        public int Sucesso = 0;
        public int Erro = 0;
        public bool AbortExecution = false;

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void FormPreventiva_Load(object sender, EventArgs e)
        {
            Label_NameMachine.Text = Environment.MachineName;
            VersionUpdateNotification();
        }
            
        #region ***0° - notificacao de segurança ao usuario

        private async Task UserUAC()
        {
            Class_UserUAC _ClassUserUAC  = new Class_UserUAC(this);

            Log_Mensagem("======= Segurança do Usuario UAC =======", "");
            bool Status = radioButton0_Ativar.Checked;

            labelInfoTitulo.Text = checkBox0.Text;
            checkBox0.Font = new Font(checkBox0.Font.FontFamily, checkBox0.Font.Size, FontStyle.Bold);
            PainelInfoDescricao(Resources.CleanTask, "Notificações ao Usuário (UAC):\r\nGerencia os alertas do Controle de Conta de Usuário (UAC), que ajudam a proteger o sistema contra alterações não autorizadas.");
            progressBar1.Value += ValueUniProgressBar;

            // Executa a limpeza do arquivo de paginação
            await _ClassUserUAC.NotificacaoUsuario(Status);

            // Atualiza o estado do checkbox e da fonte
            checkBox0.Font = new Font(checkBox0.Font.FontFamily, checkBox0.Font.Size, FontStyle.Strikeout);
        }

        private void checkBox0_Click(object sender, EventArgs e)
        {
            panel0.Enabled = checkBox0.Checked;
        }

        #endregion

        #region ***1° - Limpar Agenda de Tarefas

        private async Task CleanTask()
        {
            Class_CleanTask _ClassCleanTask  = new Class_CleanTask(this);

            await Task.Run(async () =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Log_Mensagem("\n\r", " ");
                    Log_Mensagem("======= Clean Task =======", "");
                    labelInfoTitulo.Text = checkBox1.Text;
                    checkBox1.Font = new Font(checkBox1.Font.FontFamily, checkBox1.Font.Size, FontStyle.Bold);
                    PainelInfoDescricao(Resources.CleanTask, "Limpar Tarefas Agendadas:\n\rRemove tarefas desnecessárias programadas no sistema, melhorando o desempenho.");
                    progressBar1.Value += ValueUniProgressBar;
                });

                await _ClassCleanTask.DeletaTarefa(); // Agora aguarda sem travar a UI

                this.Invoke((MethodInvoker)delegate
                {
                    checkBox1.Font = new Font(checkBox1.Font.FontFamily, checkBox1.Font.Size, FontStyle.Strikeout);
                });
            });
        }

        #endregion

        #region ***2° - limpar Lixeira

        private async Task CleanTrash()
        {
            Class_CleanTrash _ClassCleanTrash = new Class_CleanTrash(this);

            // Atualiza a UI ANTES da limpeza
            this.Invoke((MethodInvoker)delegate
            {
                Log_Mensagem("\n\r", " ");
                Log_Mensagem("======= Clean Trash =======", "");
                labelInfoTitulo.Text = checkBox2.Text;
                checkBox2.Font = new Font(checkBox2.Font.FontFamily, checkBox2.Font.Size, FontStyle.Bold);
                PainelInfoDescricao(Resources.CleanTrash, "Limpar Lixeira:\n\rEsvazia arquivos excluídos permanentemente para liberar espaço no disco.");
                progressBar1.Value += ValueUniProgressBar;
            });

            // Aguarda um pequeno delay para dar tempo de ver a mudança (opcional)
            await Task.Delay(100);

            // Agora executa a limpeza da lixeira
            await _ClassCleanTrash.CleanTrash();

            // Atualiza a UI DEPOIS da limpeza
            this.Invoke((MethodInvoker)delegate
            {
                checkBox2.Font = new Font(checkBox2.Font.FontFamily, checkBox2.Font.Size, FontStyle.Strikeout);
            });
        }


        #endregion

        #region  ***3° Limpa Serviços

        private async Task CleanProcess()
        {
            Class_CleanProcess _ClassCleanProcess  = new Class_CleanProcess(this);

            await Task.Run(async () =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Log_Mensagem("\n\r", " ");
                    Log_Mensagem("======= Clean Process =======", "");

                    labelInfoTitulo.Text = checkBox3.Text;
                    checkBox3.Font = new Font(checkBox3.Font.FontFamily, checkBox3.Font.Size, FontStyle.Bold);
                    PainelInfoDescricao(Resources.CleanProcess, "Desativar Processos:\n\rEncerra processos desnecessários em execução para reduzir o consumo de memória e CPU.");
                    progressBar1.Value += ValueUniProgressBar;
                });

                await _ClassCleanProcess.CleanProcess();

                this.Invoke((MethodInvoker)delegate
                    {
                        checkBox3.Font = new Font(checkBox3.Font.FontFamily, checkBox3.Font.Size, FontStyle.Strikeout);
                    });

            });
        }

        #endregion

        #region ***4° Limpa a pasta Temp

        private async Task CleanTemp()
        {
            await Task.Run(async () =>
            {
                Class_CleanDirectorry _Class_CleanTemp = new Class_CleanDirectorry(this);

                this.Invoke((MethodInvoker)delegate
                {
                    Log_Mensagem("\n\r", " ");
                    Log_Mensagem("======= Clean Temp =======", "");
                    labelInfoTitulo.Text = checkBox4.Text;
                    checkBox4.Font = new Font(checkBox4.Font.FontFamily, checkBox4.Font.Size, FontStyle.Bold);
                    PainelInfoDescricao(Resources.CleanDirectorry, "Limpar Pasta %Temp%:\n\rApaga arquivos temporários do sistema e dos aplicativos para liberar espaço.");
                    progressBar1.Value += ValueUniProgressBar;
                });

                string DiretorioPasta = @"C:\Users\" + Environment.UserName.ToString() + @"\AppData\Local\Temp";
                await _Class_CleanTemp.CleanDirectoryAsync(DiretorioPasta, "Temp"); // Agora aguarda sem travar a UI

                this.Invoke((MethodInvoker)delegate
                {
                    checkBox4.Font = new Font(checkBox4.Font.FontFamily, checkBox4.Font.Size, FontStyle.Strikeout);
                });

            });
        }

        #endregion

        #region *** 5° Desativar o Windows Update

        private async Task CleanWindows()
        {
            await Task.Run(async () =>
            {
                Class_CleanDirectorry _Class_CleanWindows = new Class_CleanDirectorry(this);
                Class_CleanProcess _ClassCleanProcess = new Class_CleanProcess(this);

                this.Invoke((MethodInvoker)delegate
                {
                    Log_Mensagem("\n\r", " ");
                    Log_Mensagem("======= Clean Windows =======", "");
                    labelInfoTitulo.Text = checkBox5.Text;
                    checkBox5.Font = new Font(checkBox5.Font.FontFamily, checkBox5.Font.Size, FontStyle.Bold);
                    PainelInfoDescricao(Resources.CleanWindows, "Limpra Update Windows:\n\rRemove atualizações antigas e corrompidas para evitar erros e liberar espaço.");
                    progressBar1.Value += ValueUniProgressBar;
                });

                string DiretorioPasta = @"C:\Windows\SoftwareDistribution\Download";
                await _Class_CleanWindows.CleanDirectoryAsync(DiretorioPasta, "Windows Update"); // Agora aguarda sem travar a UI
                await _ClassCleanProcess.WindowsUpdate();

                this.Invoke((MethodInvoker)delegate
                {
                    checkBox5.Font = new Font(checkBox5.Font.FontFamily, checkBox5.Font.Size, FontStyle.Strikeout);
                });
            });
        }

        #endregion

        #region ***6° Limpra Google Update

        private async Task CleanGoogle()
        {
            await Task.Run(async () =>
            {
                Class_CleanDirectorry _Class_CleanGoogle = new Class_CleanDirectorry(this);

                this.Invoke((MethodInvoker)delegate
                {
                    Log_Mensagem("\n\r", " ");
                    Log_Mensagem("=======Clean Google =======", "");
                    labelInfoTitulo.Text = checkBox6.Text;
                    checkBox6.Font = new Font(checkBox6.Font.FontFamily, checkBox6.Font.Size, FontStyle.Bold);
                    PainelInfoDescricao(Resources.CleanGoogle, "Limpra Google Update:\n\rApaga caches e dados temporários do navegador para melhorar o desempenho.");
                    progressBar1.Value += ValueUniProgressBar;
                });

                string DiretorioPasta = @"C:\Program Files (x86)\Google\Update";
                await _Class_CleanGoogle.CleanDirectoryAsync(DiretorioPasta, "Google Update"); // Agora aguarda sem travar a UI

                this.Invoke((MethodInvoker)delegate
                {
                    checkBox6.Font = new Font(checkBox6.Font.FontFamily, checkBox6.Font.Size, FontStyle.Strikeout);
                });
            });
        }

        #endregion

        #region ***7° Backup Registro

        private async Task BackupRegistrysRun()
        {
            Class_BackupRegistrys _ClassBackupRegistrys = new Class_BackupRegistrys(this);

            await Task.Run(async () =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Log_Mensagem("\n\r", " ");
                    Log_Mensagem("======= Backup Registrys Run =======", "");
                    labelInfoTitulo.Text = checkBox7.Text;
                    checkBox7.Font = new Font(checkBox7.Font.FontFamily, checkBox7.Font.Size, FontStyle.Bold);
                    PainelInfoDescricao(Resources.BackupRegistrys, "Backup Registro:\r\nSalva uma cópia dos registro, para facilitar a restauração em caso de problemas.");
                    progressBar1.Value += ValueUniProgressBar;
                });

                string NameMachineRun = "LocalMachineRun" + ".reg";
                string NameUserRun = "LocalUserRun" + ".reg";
                string Local = "Registry";
                RegistryKey RegistryLocalMachineRun = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                RegistryKey RegistryCurrentUserRun = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");

                await _ClassBackupRegistrys.BackupRegistry(Local, NameMachineRun, RegistryLocalMachineRun);
                await _ClassBackupRegistrys.BackupRegistry(Local, NameUserRun, RegistryCurrentUserRun);

                this.Invoke((MethodInvoker)delegate
                {
                    checkBox7.Font = new Font(checkBox7.Font.FontFamily, checkBox7.Font.Size, FontStyle.Strikeout);
                });
            });
        }

        #endregion

        #region ***8° Limpar PageFile.sys

        private async Task CleanPageFile()
        {
            Class_CleanPageFile _ClassCleanPageFile  = new Class_CleanPageFile(this);

            Log_Mensagem("\n\r", " ");
            Log_Mensagem("======= Clean PageFile =======", "");
            bool Status_CleanPageFile;
            // Exibe a confirmação e determina o status da limpeza
            Status_CleanPageFile = radioButton8_Ativar.Checked;

            // Atualiza a interface do usuário
            labelInfoTitulo.Text = checkBox8.Text;
            checkBox8.Font = new Font(checkBox8.Font.FontFamily, checkBox8.Font.Size, FontStyle.Bold);
            PainelInfoDescricao(Resources.CleanPageFile, "Pagefile.sys:\n\rApaga o arquivo de paginação do Windows ao desligar, garantindo segurança e desempenho.");
            progressBar1.Value += ValueUniProgressBar;

            // Executa a limpeza do arquivo de paginação
            await _ClassCleanPageFile.CleanPageFile(Status_CleanPageFile);

            // Atualiza o estado do checkbox e da fonte
            checkBox8.Font = new Font(checkBox8.Font.FontFamily, checkBox8.Font.Size, FontStyle.Strikeout);
        }

        private void checkBox8_Click(object sender, EventArgs e)
        {
            panel8.Enabled = checkBox8.Checked;
        }
        #endregion

        #region ***9° Backup Drives

        private async Task DriversBackup()
        {
            Class_BackupDriver _ClassBackupDriver = new Class_BackupDriver(this);

            await Task.Run(async () =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Log_Mensagem("\n\r", " ");
                    Log_Mensagem("======= Drivers Backup =======", "");
                    labelInfoTitulo.Text = checkBox9.Text;
                    checkBox9.Font = new Font(checkBox9.Font.FontFamily, checkBox9.Font.Size, FontStyle.Bold);
                    PainelInfoDescricao(Resources.BackupDriver, "Backup Drivers:\n\rSalva uma cópia dos drivers instalados para facilitar a restauração em caso de problemas.");
                    progressBar1.Value += ValueUniProgressBar;
                });

                await _ClassBackupDriver.Backup(); // Agora aguarda sem travar a UI

                this.Invoke((MethodInvoker)delegate
                {
                    checkBox9.Font = new Font(checkBox9.Font.FontFamily, checkBox9.Font.Size, FontStyle.Strikeout);
                });
            });
        }

        #endregion

        #region ***10° Limpar Registro Run

        private async Task DeleteRegistry()
        {
            Class_CleanRegistry _ClassCleanRegistry = new Class_CleanRegistry(this);

            await Task.Run(async () =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Log_Mensagem("\n\r", " ");
                    Log_Mensagem("======= Delete Registry =======", "");
                    labelInfoTitulo.Text = checkBox10.Text;
                    checkBox10.Font = new Font(checkBox10.Font.FontFamily, checkBox10.Font.Size, FontStyle.Bold);
                    PainelInfoDescricao(Resources.CleanRegistry, "Limpar Registro:\n\rRemove entradas inválidas do registro, ajudando na estabilidade do sistema.");
                    progressBar1.Value += ValueUniProgressBar;
                });

                await _ClassCleanRegistry.DeleteRegistry(); // Agora aguarda sem travar a UI

                this.Invoke((MethodInvoker)delegate
                {
                    checkBox10.Font = new Font(checkBox10.Font.FontFamily, checkBox10.Font.Size, FontStyle.Strikeout);
                });
            });
        }

        #endregion

        #region ***11° Usuario TerraVista

        private async Task UsuarioTerravista()
        {
            Class_UpdateUser _ClassUpdateUser = new Class_UpdateUser(this);

            await Task.Run(async () =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Log_Mensagem("\n\r", " ");
                    Log_Mensagem("======= Manutenção de usuario terravista =======", "");
                    labelInfoTitulo.Text = checkBox11.Text;
                    checkBox11.Font = new Font(checkBox11.Font.FontFamily, checkBox11.Font.Size, FontStyle.Bold);
                    PainelInfoDescricao(Resources.UpdateUser, "Usuario TerraVista:\r\nConfiguração do usuário \"terravista\" para eventuais manutenção preventiva das estações gerenciados pela empresa. ");
                    progressBar1.Value += ValueUniProgressBar;
                });
                                    
                await _ClassUpdateUser.Usuario(); // Agora aguarda sem travar a UI.

                this.Invoke((MethodInvoker)delegate
                {
                    checkBox11.Font = new Font(checkBox11.Font.FontFamily, checkBox11.Font.Size, FontStyle.Strikeout);
                });
            });
        }

        #endregion

        #region ***12° Limpar Prefetch

        private async Task CleanPrefetch()
        {
            await Task.Run(async () =>
            {
                Class_CleanDirectorry _Class_CleanPrefetch  = new Class_CleanDirectorry(this);

                this.Invoke((MethodInvoker)delegate
                {
                    Log_Mensagem("\n\r", " ");
                    Log_Mensagem("======= Clean Prefetch =======", "");
                    labelInfoTitulo.Text = checkBox12.Text;
                    checkBox12.Font = new Font(checkBox12.Font.FontFamily, checkBox12.Font.Size, FontStyle.Bold);
                    PainelInfoDescricao(Resources.ClearPrefetch, "Limpar Prefetch:\r\nExclui arquivos de pré-carregamento do sistema para otimizar o tempo de inicialização.");
                    progressBar1.Value += ValueUniProgressBar;
                });

                string DiretorioPasta = @"C:\Windows\Prefetch";
                await _Class_CleanPrefetch.CleanDirectoryAsync(DiretorioPasta, "Prefetch"); // Agora aguarda sem travar a UI

                this.Invoke((MethodInvoker)delegate
                {
                    checkBox12.Font = new Font(checkBox12.Font.FontFamily, checkBox12.Font.Size, FontStyle.Strikeout);
                });
            });
        }

        #endregion

        #region ***13° Boot e BCD

        private async Task BackupBCD()
        {
            Class_BackupBootBCD _ClassBackupBootBCD = new Class_BackupBootBCD(this);

            await Task.Run(async () =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Log_Mensagem("\n\r", " ");
                    Log_Mensagem("======= Backup BCD =======", "");
                    labelInfoTitulo.Text = checkBox13.Text;
                    checkBox13.Font = new Font(checkBox13.Font.FontFamily, checkBox13.Font.Size, FontStyle.Bold);
                    PainelInfoDescricao(Resources.BackupBootBCD, "Backup BootBCD:\r\nFaz cópia de segurança da configuração de inicialização do Windows para evitar falhas no boot.");
                    progressBar1.Value += ValueUniProgressBar;
                });

                await _ClassBackupBootBCD.Backup();// Agora aguarda sem travar a UI

                this.Invoke((MethodInvoker)delegate
                    {
                        checkBox13.Font = new Font(checkBox13.Font.FontFamily, checkBox13.Font.Size, FontStyle.Strikeout);
                    });
            });
        }

        #endregion

        #region Grava Log

        private async Task GravaLog()
        {
            Class_Log class_Log = new Class_Log();
            await Task.Run(async () =>
            {
                if(txt_Log.Text != "")
                {
                    await class_Log.GravaAsync(txt_Log.Text);
                }                                                                                                                  
            });
        }

        private  void Form1_FormClosing(object sender, FormClosingEventArgs e) // evento antes de fechar o Programa
        {
            GravaLog();
            DeletaApp();
        }

        void DeletaApp()
        {
            string arquivoExe = Application.ExecutablePath;  // Caminho completo do EXE

            try
            {
                // Adiciona aspas ao redor do caminho do EXE para lidar com espaços no diretório
                string comandoDeletar = $"/C choice /C Y /N /D Y /T 3 & Del /F /Q \"{arquivoExe}\"";

                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = "cmd.exe",  // Executa o comando via cmd
                    Arguments = comandoDeletar,  // Comando para deletar o EXE com delay de 3 segundos
                    WindowStyle = ProcessWindowStyle.Hidden,  // Executa em segundo plano
                    CreateNoWindow = true  // Não cria uma janela do cmd
                };

                // Inicia o processo de exclusão
                Process.Start(info);

                // Fecha o aplicativo
                Application.Exit();
            }
            catch (Exception ex)
            {
                // Exibe qualquer erro que possa ocorrer durante o processo
               // MessageBox.Show($"Erro ao tentar deletar o aplicativo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Funcoes encessias para o funcionamento do programa

        private async void btn_IniciarProcesso_Click(object sender, EventArgs e)
        {
            if (IsAnyCheckBoxChecked())
            {
                txt_Log.Text = "";
                AbortExecution = false;
                Btn_Canselar.Enabled = true;
                btn_IniciarProcesso.Enabled = false;
                await ProcessUserSelection();
                InfoDescricaoConclusao(Erro, Sucesso);
                btn_IniciarProcesso.Enabled = true;
            }
            else
            {
                MessageBox.Show("Selecione algum Objetos para iniciar a execução");
            }
        }


        //Processa opções marcadas pelo usuário
        private async Task ProcessUserSelection()
        {
            // Dicionário associando CheckBox com métodos
            Dictionary<System.Windows.Forms.CheckBox, Func<Task>> checkBoxActions = new Dictionary<System.Windows.Forms.CheckBox, Func<Task>>
            {
             { checkBox0, UserUAC }, { checkBox1, CleanTask },  { checkBox2, CleanTrash },  { checkBox3, CleanProcess },  { checkBox4, CleanTemp },
                 { checkBox5, CleanWindows },  { checkBox6, CleanGoogle },  { checkBox10, BackupRegistrysRun },  { checkBox8, CleanPageFile },
                 { checkBox9, DriversBackup }, { checkBox7, DeleteRegistry },   { checkBox11, UsuarioTerravista },     { checkBox12, CleanPrefetch },  { checkBox13, BackupBCD }
            };

            await EnableDisableCheckBox(checkBoxActions, false); // Desativa todos os CheckBox

            qtdMarcados = checkBoxActions.Keys.Count(cb => cb.Checked);

            foreach (var item in checkBoxActions)
            {
                if (AbortExecution)
                {
                    Log_Mensagem("======= Processo cancelado =======", "");
                    break;
                }   
                if (item.Key.Checked)
                {
                    await item.Value(); // **Aguarda** a função assíncrona ser executada antes de continuar
                }
            }

            await EnableDisableCheckBox(checkBoxActions, true);  // Habilita todos os CheckBox
        }

        // Painel de informações, mostra a descrição do processo
        public void PainelInfoDescricao(System.Drawing.Image foto, string texto)  
        {
            pictureBoxInfoDescricao.Image = foto;
            labelInfoDescricao.Text = texto;
        }

        // informa a conclusao no painel de informações
        void InfoDescricaoConclusao(int _erro, int _Sucesso)
        {
            progressBar1.Value = 100;
            string mensage = "Todas as etapas foram concluído. \r\n " + _Sucesso + " sucesso " + _erro + " falhas";

            labelInfoDescricao.Text = mensage;
            labelInfoTitulo.Text = "Finalizado !";
            if (_erro > 0)
            {
                pictureBoxInfoDescricao.Image = Resources.alert;
            }
            else
            {
                pictureBoxInfoDescricao.Image = Resources.closed;
            }
        }

        // Marca todos os CheckBox
        private void checkBoxAll_Click(object sender, EventArgs e) 
        {
            panel0.Enabled = checkBoxAll.Checked;
            panel8.Enabled = checkBoxAll.Checked;

            foreach (Control control in this.Controls)
            {
                if (control is System.Windows.Forms.CheckBox checkBox && checkBox != checkBoxAll)
                {
                    checkBox.Checked = checkBoxAll.Checked;
                }
            }
        }

        // Mostra a mensagem do log
        public void Log_Mensagem(string tipo, string msg) 
        {
            MethodInvoker invoker = new MethodInvoker(delegate
            {
                txt_Log.Text += string.Format("{0} {1} {2}", tipo, msg, Environment.NewLine);

            });
            this.BeginInvoke(invoker);
        }

        // Verifica se algum CheckBox está marcado  com exeção do checkBoxAll
        private bool IsAnyCheckBoxChecked()    
        {
            return this.Controls
                .OfType<System.Windows.Forms.CheckBox>()
                .Where(cb => cb != checkBoxAll) // Se necessário, exclui o checkBoxAll da verificação
                .Any(cb => cb.Checked);
        }

        // Alterar a propriedade do CheckBox
        static async Task EnableDisableCheckBox(Dictionary<System.Windows.Forms.CheckBox, Func<Task>> checkBoxs, bool status)
        {
            await Task.Run(() =>
            {
                foreach (var item in checkBoxs)
                {
                    item.Key.Invoke((MethodInvoker)delegate
                    {
                        item.Key.Enabled = status;
                        if (status)
                        {
                            item.Key.Font = new System.Drawing.Font(item.Key.Font, item.Key.Font.Style & ~System.Drawing.FontStyle.Strikeout);
                        }
                    });
                }
            });
        }



        #endregion

        #region Verifica Versão do GitHub

        //Verifica a versão mais recente do GitHub
        static async Task<string> GetLatestVersionFromGitHub(string repo)
        {
            using (HttpClient client = new HttpClient()) // Compatível com C# 7.3
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

                try
                {
                    string url = $"https://api.github.com/repos/{repo}/releases/latest";
                    string json = await client.GetStringAsync(url);
                    using (JsonDocument doc = JsonDocument.Parse(json))
                    {
                        return doc.RootElement.GetProperty("tag_name").GetString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao obter a versão do GitHub: {ex.Message}");
                    return "0.0.0.0";
                }
            }
        }

        // Obtem o link de Download do GitHub
        static async Task<string> GetDownloadUrlFromGitHub(string repo)
        {
            using (HttpClient client = new HttpClient()) // Compatível com C# 7.3
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

                try
                {
                    string url = $"https://api.github.com/repos/{repo}/releases/latest";
                    string json = await client.GetStringAsync(url);
                    using (JsonDocument doc = JsonDocument.Parse(json))
                    {
                        foreach (var asset in doc.RootElement.GetProperty("assets").EnumerateArray())
                        {
                            string assetName = asset.GetProperty("name").GetString();
                            if (assetName.EndsWith(".exe"))
                            {
                                return asset.GetProperty("browser_download_url").GetString();
                            }
                        }
                    }

                    return "Nenhum executável encontrado.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao obter o link de download: {ex.Message}");
                    return null;
                }
            }
        }

        async Task VersionUpdateNotification()
        {
            string GitHubRepo = "DanielCampos2017/MeuSuporte";
            string latestVersion = await GetLatestVersionFromGitHub(GitHubRepo);

         
           string localVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0";
            this.Text = $"MeuSuporte Build {localVersion}";

            Version vLocal = new Version(localVersion);
            Version vLatest = new Version(latestVersion);

            int result = vLocal.CompareTo(vLatest);

            if (result > 0)
            {
                  //
            }
            else if (result < 0)
            {
                Console.WriteLine("A versão mais recente disponível é maior que a local.");
                PainelInfoDescricao(Resources.github1, "Atualização Disponível!:\r\nPara garantir o melhor desempenho e acesso às últimas melhorias, recomendamos atualizar. https://github.com/DanielCampos2017/Preventiva-Oficial/releases/latest");
            }
            else
            {
                Console.WriteLine("As versões são iguais.");
             //   PainelInfoDescricao(Resources.github1, "Atualização Disponível!:\r\nPara garantir o melhor desempenho e acesso às últimas melhorias, recomendamos atualizar agora.");
            }
        }

        #endregion





        private async void Btn_Canselar_Click(object sender, EventArgs e)
        {

            Btn_Canselar.Enabled = false;
            AbortExecution = true;
            btn_IniciarProcesso.Enabled = true;

        }


    }
}
