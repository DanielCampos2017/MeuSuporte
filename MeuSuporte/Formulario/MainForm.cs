using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using MeuSuporte.Properties;
using Microsoft.Win32;
using Control = System.Windows.Forms.Control;
using Font = System.Drawing.Font;
using Point = System.Drawing.Point;



namespace MeuSuporte
{
    public partial class MainForm : System.Windows.Forms.Form
    {
       
        public int ValueUniProgressBar = 100;
        public int Sucesso = 0;
        public int Erro = 0;
        private bool Aborted = false;
        private string UserName;
        private CancellationTokenSource cts;
        CancellationToken token;
        WinGlobal_UIService UIService;

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            UIService = new WinGlobal_UIService(this, txt_Log, progressBar1, ValueUniProgressBar); // envia os componete para interface
            
            WinGlobal_UIService2.Initialize(this, txt_Log, progressBar1, ValueUniProgressBar);
        }

        #region Funçoes de UI

        // Painel de informações, mostra a descrição do processo
        public void PainelInfoDescricao(Image foto, string texto)
        {
            pictureBoxInfoDescricao.Image = foto;
            labelInfoDescricao.Text = texto;
        }

        // informa a conclusao no painel de informações
        void InfoDescricaoConclusao(int _erro, int _Sucesso)
        {
            if (Aborted == false)
            {
                string mensage = "Todas as etapas foram concluído. \r\n " + _Sucesso + " sucesso " + _erro + " falhas";
                labelInfoDescricao.Text = mensage;
                labelInfoTitulo.Text = "Finalizado !";

                if (_erro > 0)
                {
                    pictureBoxInfoDescricao.Image = Resources.Alert_Black;
                }
                else
                {
                    pictureBoxInfoDescricao.Image = Resources.closed_Black;
                }
            }
            else
            {
                string mensage = "Processo abortado sem conclusão. \r\n " + _Sucesso + " sucesso " + _erro + " falhas";
                labelInfoDescricao.Text = mensage;
                labelInfoTitulo.Text = "Abortado !";

                pictureBoxInfoDescricao.Image = Resources.abort_Black;
            }
            progressBar1.Value = 100;
        }

        //Adiciona valor ao ProgressBar
        public void ProgressBarADD(int valor)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() => ProgressBarADD(valor)));             
                return;
            }

            int _max = progressBar1.Value + valor;

            if (_max < 100)
            {
                progressBar1.Value += valor;
            }
            else
            {
                progressBar1.Value = 100;
            }
        }

        //Evento Load do Formulario
        private void FormPreventiva_Load(object sender, EventArgs e)
        {           
            Label_NameMachine.Text = Environment.MachineName;
            ExecutionPath(); // verifica o local da execução do programa
            CheckBuild();   // verifica a versão do programa
            UserNameCheckBox();
        }

        //Evento Closing do Formulario
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) // evento antes de fechar o Programa
        {
            GravaLog();
            DeletaApp();
        }

        //Adiciona informaçoes do progresso na interface
        async Task UpdateIfonUI(string _Log, CheckBox _CheckBox, Image _icon, string _description)
        {
            await Log_MensagemAsync("\r\n", true);
            await Log_MensagemAsync($"======= {_Log} =======", true);
            await Log_MensagemAsync(" ", false);
            labelInfoTitulo.Text = _CheckBox.Text;
            _CheckBox.Font = new Font(checkBox_UserUAC.Font.FontFamily, checkBox_UserUAC.Font.Size, FontStyle.Bold);
            pictureBoxInfoDescricao.Image = _icon;
            labelInfoDescricao.Text = _description;                      
        }

        //Adiciona informaçoes do progresso Abortado na interface
        private string CurrentProcess; // Processo atual em execução
        private List<string> ProcessoSelecionados = new List<string>(); // lista de processo para ser executado
        


        //Exibe um log com todos os Processos que foram executadoes e os que foram canselado
        async Task MensagemAbortedAsync()
        {
            bool executed = true;

            foreach (string _Porecesso in ProcessoSelecionados)
            {
                if (executed == true)
                {
                    await Log_MensagemAsync($" • {_Porecesso}   {{executed}}", true);

                    if (_Porecesso == CurrentProcess)
                    {
                        await Log_MensagemAsync($" • {_Porecesso}   {{aborted}}", true);
                        executed = false;
                    }
                }
                else
                {
                    await Log_MensagemAsync($" • {_Porecesso}   {{canceled}}", true);
                }
            }
        }

        public async Task Log_MensagemAsyncold(string mensagem, bool pularLinha, bool sobrescrev = false)
        {
            if (sobrescrev)
            {
                if (txt_Log.InvokeRequired)
                {
                    await Task.Run(() =>
                    {
                        txt_Log.Invoke(new Action(() =>
                        {
                            string[] linhas = txt_Log.Text.Split('\n');
                            txt_Log.Text = string.Join("\n", linhas.Take(linhas.Length - 1).Concat(new[] { mensagem }));
                        }));
                    });
                }
                else
                {
                    string[] linhas = txt_Log.Text.Split('\n');
                    txt_Log.Text = string.Join("\n", linhas.Take(linhas.Length - 1).Concat(new[] { mensagem }));
                }
            }
            else
            {
                if (txt_Log.InvokeRequired)
                {
                    await Task.Run(() =>
                    {
                        txt_Log.Invoke(new Action(() =>
                        {
                            txt_Log.AppendText(mensagem);
                            if (pularLinha)
                                txt_Log.AppendText(Environment.NewLine);
                        }));
                    });
                }
                else
                {
                    txt_Log.AppendText(mensagem);
                    if (pularLinha)
                        txt_Log.AppendText(Environment.NewLine);
                }
            }
        }

        public async Task Log_MensagemAsync(string mensagem, bool pularLinha)
        {
            if (txt_Log.InvokeRequired)
            {
                await Task.Run(() =>
                {
                    txt_Log.Invoke(new Action(() =>
                    {
                        txt_Log.AppendText(mensagem);
                        if (pularLinha)
                            txt_Log.AppendText(Environment.NewLine);
                    }));
                });
            }
            else
            {
                txt_Log.AppendText(mensagem);
                if (pularLinha)
                    txt_Log.AppendText(Environment.NewLine);
            }
        }

     

        // Ajusta Layout das CheckBox e 
        private Dictionary<Control, Point> posicoesOriginais = new Dictionary<Control, Point>();
        private bool posicoesSalvas = false;
        private int panelPosition = 18;
        private void AjustarLayout()
        {
            // Salvar posições apenas uma vez
            if (!posicoesSalvas)
            {
                //foreach (Control ctrl in this.Controls)
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is CheckBox || ctrl is Panel || ctrl is PictureBox)
                        posicoesOriginais[ctrl] = ctrl.Location;
                }
                posicoesSalvas = true;

            }

            // Começa reposicionando tudo de acordo com a posição original
            foreach (var item in posicoesOriginais)
            {
                item.Key.Location = item.Value;
            }

            // Adiciona espaço para os painéis abertos
            int offset = 18; // altura dos painéis
            int acumuladorY = 0;

            // CheckBoxes e PictureBoxes em ordem
            CheckBox[] checkBoxes = new CheckBox[] {
             checkBox_UserUAC, checkBox_CleanTask,  checkBox_CleanTrash,
             checkBox_CleanProcess,  checkBox_CleanTemp, checkBox_CleanWindowsUpdate,
             checkBox_CleanGoogle,  checkBox_BackupRegistrysRun, checkBox_CleanPageFile,
             checkBox_DriversBackup,  checkBox_DeleteRegistry,  checkBox_Usuario,
             checkBox_CleanPrefetch,  checkBox_BackupBCD,  checkBox_RestorePoint,  checkBox_ConnectionRDP, checkBox_Bloatware
            };

            PictureBox[] pictureBoxes = new PictureBox[] {
              pictureBox0, pictureBox1, pictureBox2, pictureBox3, pictureBox4,
              pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9,
              pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16
            };

            for (int i = 0; i < checkBoxes.Length; i++)
            {
                CheckBox atual = checkBoxes[i];
                PictureBox imagem = pictureBoxes[i];

                // Ajusta posição do checkbox
                Point posCheck = posicoesOriginais[atual];
                atual.Location = new Point(posCheck.X, posCheck.Y + acumuladorY);

                // Ajusta posição da imagem associada
                Point posImg = posicoesOriginais[imagem];
                imagem.Location = new Point(posImg.X, posImg.Y + acumuladorY);

                // Verifica se o checkbox exige painel e se está marcado
                if (atual == checkBox_UserUAC && atual.Checked)
                {
                    panel_UserUAC.Location = new Point(atual.Location.X + 20, atual.Location.Y + panelPosition);
                    panel_UserUAC.Visible = true;
                    acumuladorY += offset;
                }
                else if (atual == checkBox_CleanPageFile && atual.Checked)
                {
                    panel_CleanPageFile.Location = new Point(atual.Location.X + 20, atual.Location.Y + panelPosition);
                    panel_CleanPageFile.Visible = true;
                    acumuladorY += offset;
                }
                else if (atual == checkBox_ConnectionRDP && atual.Checked)
                {
                    panel_ConnectionRDP.Location = new Point(atual.Location.X + 20, atual.Location.Y + panelPosition);
                    panel_ConnectionRDP.Visible = true;
                    acumuladorY += offset;
                }
            }

            // Esconde os painéis que não foram ativados
            if (!checkBox_UserUAC.Checked) panel_UserUAC.Visible = false;
            if (!checkBox_CleanPageFile.Checked) panel_CleanPageFile.Visible = false;
            if (!checkBox_ConnectionRDP.Checked) panel_ConnectionRDP.Visible = false;
        }

        #endregion

        #region Funçoes dos Botões UI

        //CheckBox Marca todos
        private void checkBoxAll_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox checkBox && checkBox != checkBoxAll)
                {
                    checkBox.Checked = checkBoxAll.Checked;
                }
            }
            AjustarLayout();
        }

        //CheckBox Notificação UAC
        private void checkBox_UserUAC_Click(object sender, EventArgs e)
        {
            AjustarLayout();
        }

        //CheckBox Acesso Remoto RDP
        private void checkBox_ConnectionRDP_Click(object sender, EventArgs e)
        {
            AjustarLayout();
        }

        //CheckBox PageFile
        private void checkBox_CleanPageFile_Click(object sender, EventArgs e)
        {
            AjustarLayout();
        }

        //Botão de Iniciar
        private async void btn_IniciarProcesso_Click(object sender, EventArgs e)
        {
            if (IsAnyCheckBoxChecked()) // verifica se tem algum CheckBox selecionado
            {
                progressBar1.Value = 0;
                txt_Log.Text = "";
                Btn_Canselar.Enabled = true;
                btn_IniciarProcesso.Enabled = false;
                await ExecuteProcesses();
                //InfoDescricaoConclusao(UIService.Erro, UIService.Sucesso);
                InfoDescricaoConclusao(WinGlobal_UIService2.Instance.Erro, WinGlobal_UIService2.Instance.Sucesso);
                btn_IniciarProcesso.Enabled = true;
            }
            else
            {
                MessageBox.Show("Selecione algum Objetos para iniciar a execução");
            }
        }

        //Botão de Canselar
        private async void Btn_Canselar_Click(object sender, EventArgs e)
        {
            cts?.Cancel(); // cansela as trarefas 
            Btn_Canselar.Enabled = false;
            btn_IniciarProcesso.Enabled = true;
        }


        #endregion
         
        #region Politica de Seguraça do Software

        //Verifica Versão do GitHub
        async Task CheckBuild()
        {
            CheckBuild_Mananger _Class_BuildView = new CheckBuild_Mananger();
            _Class_BuildView.Build();
        }

        // função que desativa os checkbox chamado atraves da Class_BuildView
        public void checkBoxAllState(bool state)
        {
            panel_UserUAC.Enabled = state;
            panel_CleanPageFile.Enabled = state;
            panel_ConnectionRDP.Enabled = state;
            btn_IniciarProcesso.Enabled = state;

            foreach (Control control in this.Controls)
            {
                if (control is CheckBox checkBox)
                {
                    checkBox.Enabled = state;
                }
            }
        }

        //Verifica se o programa esta execuntando localmente ou em rede
        private void ExecutionPath()
        {
            WinApp_ExecutionPath _Class_ExecutionPath = new WinApp_ExecutionPath();
            if (_Class_ExecutionPath.getDiscoverNetwork())
            {
                MessageBox.Show("O programa não pode ser executado em um diretório de rede. Por favor, execute-o localmente em sua maquina.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0); // Força o fechamento imediato
                return;
            }
        }

        // adiciona o nome do usuario na checkBox_Usuario        
        async Task UserNameCheckBox()
        {
            //Password_Private _ClassPassword = new Password_Private();
            Password_Public _ClassPassword = new Password_Public();
            UserName = _ClassPassword.GetUser();
            checkBox_Usuario.Text = "Usuario " + UserName;
        }

        #endregion

        #region notificacao de segurança ao usuario UAC

        private async Task UserUAC()
        {
            WinUserUAC_Mananger UAC_Mananger = new WinUserUAC_Mananger();           

            // Atualiza a UI antes da execução assíncrona
            await UpdateIfonUI("Segurança do Usuario UAC", checkBox_UserUAC, Resources.UserUAC_Black, "Notificações ao Usuário (UAC):\r\nGerencia os alertas do Controle de Conta de Usuário (UAC), que ajudam a proteger o sistema contra alterações não autorizadas.");
            await Task.Delay(800);

            // Executa a função assíncrona sem bloquear a UI
            await UAC_Mananger.Mananger(radioButtonUserUAC_Ativar.Checked, ValueUniProgressBar);
            await Task.Delay(1000);
                       
            checkBox_UserUAC.Font = new Font(checkBox_UserUAC.Font.FontFamily, checkBox_UserUAC.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Limpar Agenda de Tarefas

        private async Task CleanTask()
        {
            WinTask_Mananger Task_Mananger = new WinTask_Mananger();

            // Atualiza a UI antes da execução assíncrona            
            await UpdateIfonUI("Clean Task", checkBox_CleanTask, Resources.CleanTask_Black, "Limpar Tarefas Agendadas:\n\rRemove tarefas desnecessárias programadas no sistema, melhorando o desempenho.");

            await Task.Delay(800);
            await Task.Run(() => Task_Mananger.Mananger(token, ValueUniProgressBar), token); // Executa a tarefa async em uma nova thread

            await Task.Delay(1000);
            checkBox_CleanTask.Font = new Font(checkBox_CleanTask.Font.FontFamily, checkBox_CleanTask.Font.Size, FontStyle.Strikeout);

        }

        #endregion

        #region limpar Lixeira

        private async Task CleanTrash()
        {
            WinTrash_Mananger Trash_Mananger = new WinTrash_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await UpdateIfonUI("Clean Trash", checkBox_CleanTrash, Resources.CleanTrash_Black, "Limpar Lixeira:\n\rEsvazia arquivos excluídos permanentemente para liberar espaço no disco.");
            await Task.Delay(800);

            await Task.Run(() => Trash_Mananger.Mananger(token, ValueUniProgressBar), token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);
                        
            checkBox_CleanTrash.Font = new Font(checkBox_CleanTrash.Font.FontFamily, checkBox_CleanTrash.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Limpa Serviços

        private async Task CleanProcess()
        {
            WinService_Mananger Service_Mananger = new WinService_Mananger();
            WinService_List Service_List = new WinService_List();

            // Atualiza a UI antes da execução assíncrona
            await UpdateIfonUI("Clean Process", checkBox_CleanProcess, Resources.CleanProcess_Black, "Desativar Processos:\n\rEncerra processos desnecessários em execução para reduzir o consumo de memória e CPU.");
            await Task.Delay(800);

            await Task.Run(() => Service_Mananger.Mananger(Service_List.All,ValueUniProgressBar, false, token), token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            // Atualiza a UI após a execução assíncrona  
            checkBox_CleanProcess.Font = new Font(checkBox_CleanProcess.Font.FontFamily, checkBox_CleanProcess.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Limpa a pasta Temp

        
        private async Task CleanTemp()
        {
            WinDirectory_Mananger Directory_Mananger = new WinDirectory_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await UpdateIfonUI("Clean Temp", checkBox_CleanTemp, Resources.CleanDirectorry_Black, "Limpar Pasta %Temp%:\n\rApaga arquivos temporários do sistema e dos aplicativos para liberar espaço.");
            await Task.Delay(800);
                    
            string DiretorioPasta = @"C:\Users\" + Environment.UserName.ToString() + @"\AppData\Local\Temp";
            await Task.Run(() => Directory_Mananger.Mananger(DiretorioPasta, "%Temp%", ValueUniProgressBar, token), token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            // Atualiza a UI após a execução assíncrona
            checkBox_CleanTemp.Font = new Font(checkBox_CleanTemp.Font.FontFamily, checkBox_CleanTemp.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Desativar o Windows Update

        private async Task CleanWindowsUpdate()
        {
            WinDirectory_Mananger Directory_Mananger = new WinDirectory_Mananger();
            WinService_Mananger Service_Mananger = new WinService_Mananger();
            WinService_List Service_List = new WinService_List();

            // Atualiza a UI antes da execução assíncrona
            await UpdateIfonUI("Clean Windows", checkBox_CleanWindowsUpdate, Resources.CleanTask_Black, "Desativar Update Windows:\n\rRemove atualizações antigas e corrompidas para evitar erros e liberar espaço e Desativa o serviço de Update");
            await Task.Delay(800);

            string DiretorioPasta = @"C:\Windows\SoftwareDistribution\Download";
            await Task.Run(() => Directory_Mananger.Mananger(DiretorioPasta, "Windows Update", ValueUniProgressBar / 2, token), token); // Executa a tarefa async em uma nova thread
            await Task.Run(() => Service_Mananger.Mananger(Service_List.WindowsUpdate, ValueUniProgressBar /2, true, token), token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            // Atualiza a UI após a execução assíncrona
            checkBox_CleanWindowsUpdate.Font = new Font(checkBox_CleanWindowsUpdate.Font.FontFamily, checkBox_CleanWindowsUpdate.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Limpra Google Update

        private async Task CleanGoogle()
        {
            WinDirectory_Mananger Directory_Mananger = new WinDirectory_Mananger();
            WinService_Mananger Service_Mananger = new WinService_Mananger();
            WinService_List Service_List = new WinService_List();

            // Atualiza a UI antes da execução assíncrona
            await UpdateIfonUI("Clean Google", checkBox_CleanGoogle, Resources.CleanGoogle_Black, "Limpra Google Update:\n\rApaga caches e dados temporários do navegador para melhorar o desempenho.");
            await Task.Delay(800);

            await Task.Run(() => Service_Mananger.Mananger(Service_List.Google, ValueUniProgressBar /2, false, token), token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            string DiretorioPasta = @"C:\Program Files (x86)\Google\Update";
            await Task.Run(() => Directory_Mananger.Mananger(DiretorioPasta, "Google Update", ValueUniProgressBar / 2, token), token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            // Atualiza a UI após a execução assíncrona
            checkBox_CleanGoogle.Font = new Font(checkBox_CleanGoogle.Font.FontFamily, checkBox_CleanGoogle.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Backup Registro

        private async Task BackupRegistrysRun()
        {
            WinRegistryBackup_Mananger RegistryBackup_Mananger = new WinRegistryBackup_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await UpdateIfonUI("Backup Registrys Run", checkBox_BackupRegistrysRun, Resources.BackupRegistrys_Black, "Backup Registro:\r\nSalva uma cópia dos registro, para facilitar a restauração em caso de problemas.");
            await Task.Delay(800);

            await Task.Run(() => RegistryBackup_Mananger.Mananger(ValueUniProgressBar, token), token);
            await Task.Delay(1000);
                        
            checkBox_BackupRegistrysRun.Font = new Font(checkBox_BackupRegistrysRun.Font.FontFamily, checkBox_BackupRegistrysRun.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Limpar PageFile.sys

        private async Task CleanPageFile()
        {
            WinPageFile_Mananger PageFile_Mananger = new WinPageFile_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await UpdateIfonUI("Clean PageFile", checkBox_CleanPageFile, Resources.CleanPageFile_Black, "Pagefile.sys:\n\rApaga o arquivo de paginação do Windows ao desligar, garantindo segurança e desempenho.");
            await Task.Delay(800);

            // Executa a função assíncrona sem bloquear a UI
            await Task.Run(() => PageFile_Mananger.Mananger(radioButtonCleanPageFile_Ativar.Checked, ValueUniProgressBar, token), token);
            await Task.Delay(1000);

            checkBox_CleanPageFile.Font = new Font(checkBox_CleanPageFile.Font.FontFamily, checkBox_CleanPageFile.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Backup Drives

        private async Task DriversBackup()
        {
            WinBackupDriver_Mananger BackupDriver_Mananger = new WinBackupDriver_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await UpdateIfonUI("Drivers Backup", checkBox_DriversBackup, Resources.BackupDriver_Black, "Backup Drivers:\n\rSalva uma cópia dos drivers instalados para facilitar a restauração em caso de problemas.");
            await Task.Delay(800);

            await Task.Run(() => BackupDriver_Mananger.Mananger(token), token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            // Atualiza a UI após a execução assíncrona  
            checkBox_DriversBackup.Font = new Font(checkBox_DriversBackup.Font.FontFamily, checkBox_DriversBackup.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Limpar Registro Run

        private async Task DeleteRegistry()
        {
            WinRegistryBin_Mananger _ClassCleanRegistry = new WinRegistryBin_Mananger();
      
            // Atualiza a UI antes da execução assíncrona
            await UpdateIfonUI("Delete Registry", checkBox_DeleteRegistry, Resources.CleanRegistry_Black, "Limpar Registro:\n\rRemove entradas inválidas do registro, ajudando na estabilidade do sistema.");
            await Task.Delay(800);
                        
            await Task.Run(() => _ClassCleanRegistry.DeleteRegistry(token, ValueUniProgressBar), token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            checkBox_DeleteRegistry.Font = new Font(checkBox_DeleteRegistry.Font.FontFamily, checkBox_DeleteRegistry.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Usuario Suporte

        private async Task UsuarioSuporte()
        {
            WinUser_Mananger User_Mananger = new WinUser_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await UpdateIfonUI($"Manutenção de usuario {UserName}", checkBox_Usuario, Resources.UpdateUser_Black, $"Usuario {UserName}:\r\nConfiguração do usuário \"{UserName}\" para eventuais manutenção preventiva das estações gerenciados pela empresa. ");
            await Task.Delay(800);

            await Task.Run(() => User_Mananger.Mananger(token, ValueUniProgressBar), token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            checkBox_Usuario.Font = new Font(checkBox_Usuario.Font.FontFamily, checkBox_Usuario.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Limpar Prefetch

        private async Task CleanPrefetch()
        {
            WinDirectory_Mananger Directory_Mananger = new WinDirectory_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await UpdateIfonUI("Clean Prefetch", checkBox_CleanPrefetch, Resources.ClearPrefetch_Black, "Limpar Prefetch:\r\nExclui arquivos de pré-carregamento do sistema para otimizar o tempo de inicialização.");
            await Task.Delay(800);

            // Executa a função assíncrona sem bloquear a UI
            string DiretorioPasta = @"C:\Windows\Prefetch";
            await Task.Run(() => Directory_Mananger.Mananger(DiretorioPasta, "Prefetch", ValueUniProgressBar, token), token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            // Atualiza a UI após a execução assíncrona    
            checkBox_CleanPrefetch.Font = new Font(checkBox_CleanPrefetch.Font.FontFamily, checkBox_CleanPrefetch.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Boot e BCD

        private async Task BackupBCD()
        {
            WinBackupBCD_Mananger BackupBCD_Mananger = new WinBackupBCD_Mananger();
          
            // Atualiza a UI antes da execução assíncrona
            await UpdateIfonUI("Backup BCD", checkBox_BackupBCD, Resources.BackupBootBCD_Black, "Backup BootBCD:\r\nFaz cópia de segurança da configuração de inicialização do Windows para evitar falhas no boot.");
            await Task.Delay(800);

            await Task.Run(() => BackupBCD_Mananger.Mananger(token, ValueUniProgressBar), token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            checkBox_BackupBCD.Font = new Font(checkBox_BackupBCD.Font.FontFamily, checkBox_BackupBCD.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Proteção de Propriedades do Sistema

        private async Task CreateSystemPointOld()
        {
            await UpdateIfonUI("Proteção do Sistema", checkBox_RestorePoint, Resources.restore_Black, checkBox_RestorePoint.Text + "Ponto de Restauração\n\rpermite reverter o sistema para um estado anterior, revertendo configurações, programas e arquivos do sistema");
            await Task.Delay(800);

            await CreateSystemPoint("Old");
            await Task.Delay(1000);
          
            checkBox_RestorePoint.Font = new Font(checkBox_DriversBackup.Font.FontFamily, checkBox_DriversBackup.Font.Size);
        }

        private async Task CreateSystemPointNew()
        {
            await UpdateIfonUI("Proteção do Sistema", checkBox_RestorePoint, Resources.restore_Black, checkBox_RestorePoint.Text + "Ponto de Restauração\n\rpermite reverter o sistema para um estado anterior, revertendo configurações, programas e arquivos do sistema");
            await Task.Delay(800);

            await CreateSystemPoint("New");
            await Task.Delay(1000);
             
            checkBox_RestorePoint.Font = new Font(checkBox_RestorePoint.Font.FontFamily, checkBox_RestorePoint.Font.Size, FontStyle.Strikeout);
        }

        private async Task CreateSystemPoint(string State)
        {
            WinRestorePoint_IsSystemRestoreEnabled _IsSystemRestoreEnabled = new WinRestorePoint_IsSystemRestoreEnabled();

            //1° verifica se a configuração esta ativa
            if (!await _IsSystemRestoreEnabled.IsSystemRestoreEnabledAsync())
            {
                // ativa a configuração
                WinRestorePoint_EnableProtection _Class_EnableProtection = new WinRestorePoint_EnableProtection();
                await _Class_EnableProtection.EnableProtection(ValueUniProgressBar / 3);
                await Task.Delay(1000);
            }

            // Gera uma Descrição para o Ponto de Restauração
            WinRestorePoint_PointName _PointName = new WinRestorePoint_PointName();
            string NamePoint = _PointName.GetName(State);
            await Task.Delay(200);

            //2° Cria Ponto de Restauração
            WinRestorePoint_Create _PointCreate = new WinRestorePoint_Create();
            await Task.Run(() => _PointCreate.CreatePoint(NamePoint, ValueUniProgressBar / 3, token), token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            //3° Procura pelo Ponto de Restauração recém-criado
            WinRestorePoint_PointSearch _PointSearch = new WinRestorePoint_PointSearch();
            await Task.Run(() => _PointSearch.PointSearch(NamePoint, ValueUniProgressBar / 3, token), token); // Executa a tarefa async em uma nova thread                 
        }

        #endregion

        #region Acesso Remoto RDP

        private async Task RemoteRDP()
        {
            WinRemoteRDP_Mananger RemoteRDP_Mananger = new WinRemoteRDP_Mananger();
            ProgressBarADD(ValueUniProgressBar / 2);

            // Atualiza a UI antes da execução assíncrona
            UpdateIfonUI("Remote Desktop Connection", checkBox_ConnectionRDP, Resources.Remoto_Black, "Remote Desktop Connection RDP\r\nPermite outros computadores na mesma rede se conecta via TS em sua maquina para manutenções remota");            
            await Task.Delay(800);
            
            await Task.Run(() => RemoteRDP_Mananger.Mananger(radioButtonradioButtonConnectionRDP_Ativar.Checked, token, ValueUniProgressBar), token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            ProgressBarADD(ValueUniProgressBar / 2);
            // Atualiza a UI após a execução assíncrona
            checkBox_BackupBCD.Font = new Font(checkBox_BackupBCD.Font.FontFamily, checkBox_BackupBCD.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Bloatware

        private async Task RenoveBloatware()
        {
            await Task.Run(() => check());
        }
              
        async Task check()
        {
            WinBloatware_Mananger RemoveBloatware = new WinBloatware_Mananger();  
            await UpdateIfonUI("Remover Bloatware", checkBox_Bloatware, Resources.Store_Black, "Bloatware:\r\nSoftwares pré-instalados que nem sempre são úteis para o usuário, como alguns jogos ou versões de teste de programas pagos.");
            await Task.Delay(800);

            await Task.Run(() => RemoveBloatware.Mananger(token), token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            // Atualiza a UI após a execução assíncrona    
            checkBox_Bloatware.Font = new Font(checkBox_Bloatware.Font.FontFamily, checkBox_Bloatware.Font.Size, FontStyle.Strikeout);            
        }

        public async Task Bloatware_ExtractProgress(int valor)
        {
            

            int valorMaximo = (valor > 1) ? Convert.ToInt32(valor) : 100;

            float unidade = (float)ValueUniProgressBar / (float)valorMaximo;

            ProgressBarADD(await ValueUnitBloatware(unidade));
        }

        private static float Bloatware_accumulator = 0f; // Variável para armazenar o valor acumulado    
        static async Task<int> ValueUnitBloatware(float valor)
        {
            Bloatware_accumulator += valor;

            // Extrai a parte inteira e armazena na ProgressBar
            int parteInteira = (int)Bloatware_accumulator;

            if (parteInteira > 0)
            {
                Bloatware_accumulator -= parteInteira;
                return parteInteira;
            }
            return 0;
        }



        #endregion

        #region Funcao de Encerramento do Programa

        // Grava Log em um arquivo de Texto
        private async Task GravaLog()
        {
            WinApp_Log class_Log = new WinApp_Log();
            await Task.Run(async () =>
            {
                if (txt_Log.Text != "")
                {
                    await class_Log.GravaAsync(txt_Log.Text);
                }
            });
        }

        //Deleta o Aplicativo
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

                Process.Start(info); // Inicia o processo de exclusão
                Application.Exit(); // Fecha o aplicativo
            }
            catch (Exception ex)
            {
                Application.Exit();
            }
        }

        #endregion

        #region Executa todas as tarefas dos CheckBox selecionado

        //Processa opções marcadas pelo usuário
        private async Task ExecuteProcesses()
        {
            // Dicionário associando CheckBox com métodos
            Dictionary<CheckBox, Func<Task>> checkBoxActions = new Dictionary<CheckBox, Func<Task>>
                {
                  {checkBox_UserUAC, UserUAC}, {checkBox_CleanTask, CleanTask},  {checkBox_CleanTrash, CleanTrash},  {checkBox_CleanProcess, CleanProcess},  {checkBox_CleanTemp, CleanTemp},
                  {checkBox_CleanWindowsUpdate, CleanWindowsUpdate},  {checkBox_CleanGoogle, CleanGoogle}, {checkBox_BackupRegistrysRun, BackupRegistrysRun}, {checkBox_CleanPageFile, CleanPageFile},
                  {checkBox_DriversBackup, DriversBackup}, {checkBox_DeleteRegistry, DeleteRegistry}, {checkBox_Usuario, UsuarioSuporte},{checkBox_CleanPrefetch, CleanPrefetch},  {checkBox_BackupBCD, BackupBCD},
                  {checkBox_RestorePoint, CreateSystemPointNew}, {checkBox_ConnectionRDP, RemoteRDP}, {checkBox_Bloatware, RenoveBloatware }
                };

            try
            {
                cts = new CancellationTokenSource();
                token = cts.Token;

                // Desativa todos os CheckBox
                await EnableDisableCheckBox(checkBoxActions, false);

                // Fraciona o valor em 100
                ValueUniProgressBar = 100 / checkBoxActions.Keys.Count(cb => cb.Checked);

                // cria um ponto de restauração antes de todas as alterações caso essa opção esteja marcada 
                if (checkBox_RestorePoint.Checked == true)
                {
                    await CreateSystemPointOld();
                }

                //Lista uma Variavel com todo os processo que sera executado
                foreach (var item in checkBoxActions)
                {
                    if (item.Key.Checked == true) 
                    {                        
                        ProcessoSelecionados.Add(item.Key.Text); // adiciona na lista somente os que estiver marcado pelo usuario
                    }
                }

                // executa todos os processo que estiver selecionados
                foreach (var item in checkBoxActions)
                {                   
                    // Interrompe o loop imediatamente, caso alguma função não tenha passado CancellationToken em sua função então encerra por aqui
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                    
                    //Execulta a função do CheckBox marcado
                    if (item.Key.Checked)
                    {
                        CurrentProcess = item.Key.Text; // adiciona na variavel o processa que ira ser executado 
                        await item.Value();                                                
                    }
                }

                // Habilita todos os CheckBox
                await EnableDisableCheckBox(checkBoxActions, true);
            }
            catch (OperationCanceledException ex)
            {      
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);// Mensagem informativa de canselamento

                await Log_MensagemAsync("\r\n", true);
                await Log_MensagemAsync($"======= PROCESSO CANCELADO PELO USUÁRIO ! =======", false);
                await Log_MensagemAsync(" ", true);
                await MensagemAbortedAsync(); // Exibe um log com todos os Processos que foram executadoes e os que foram canselado
                await Task.Delay(300);

                await EnableDisableCheckBox(checkBoxActions, true); // Habilita todos os CheckBox
                await Task.Delay(300);
                Aborted = true;
            }
        }


        // Verifica se algum CheckBox está marcado  com exeção do checkBoxAll
        private bool IsAnyCheckBoxChecked()
        {
            return this.Controls
                .OfType<CheckBox>()
                .Where(cb => cb != checkBoxAll) // Se necessário, exclui o checkBoxAll da verificação
                .Any(cb => cb.Checked);
        }

        // Alterar a propriedade do CheckBox
        static async Task EnableDisableCheckBox(Dictionary<CheckBox, Func<Task>> checkBoxs, bool status)
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
                            item.Key.Font = new Font(item.Key.Font.FontFamily, item.Key.Font.Size, FontStyle.Regular);
                        }
                    });
                }
            });
        }








        #endregion

       
    }
}
