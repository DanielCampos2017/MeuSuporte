﻿using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using MeuSuporte.Properties;

namespace MeuSuporte
{
    internal class WinApp_Mananger
    {
        #region Dependencias

        private static WinApp_Mananger _instance;

        private CheckBox checkBox_UserUAC;
        private CheckBox checkBox_CleanTask;
        private CheckBox checkBox_CleanTrash;
        private CheckBox checkBox_CleanProcess;
        private CheckBox checkBox_CleanTemp;
        private CheckBox checkBox_CleanWindowsUpdate;
        private CheckBox checkBox_CleanGoogle;
        private CheckBox checkBox_BackupRegistrysRun;
        private CheckBox checkBox_CleanPageFile;
        private CheckBox checkBox_DriversBackup;
        private CheckBox checkBox_DeleteRegistry;
        private CheckBox checkBox_Usuario;
        private CheckBox checkBox_CleanPrefetch;
        private CheckBox checkBox_BackupBCD;
        private CheckBox checkBox_RestorePoint;
        private CheckBox checkBox_ConnectionRDP;
        private CheckBox checkBox_Bloatware;

        private RadioButton radioButtonUserUAC_Ativar;
        private RadioButton radioButtonCleanPageFile_Ativar;
        private RadioButton radioButtonradioButtonConnectionRDP_Ativar;

        #endregion

        #region instância
        public static void Initialize(CheckBox _checkBox_UserUAC, CheckBox _checkBox_CleanTask, CheckBox _checkBox_CleanTrash, 
            CheckBox _checkBox_CleanProcess, CheckBox _checkBox_CleanTemp, CheckBox _checkBox_CleanWindowsUpdate, CheckBox _checkBox_CleanGoogle, 
            CheckBox _checkBox_BackupRegistrysRun, CheckBox _checkBox_CleanPageFile, CheckBox _checkBox_DriversBackup, CheckBox _checkBox_DeleteRegistry, 
            CheckBox _checkBox_Usuario, CheckBox _checkBox_CleanPrefetch, CheckBox _checkBox_BackupBCD, CheckBox _checkBox_RestorePoint, 
            CheckBox _checkBox_ConnectionRDP, CheckBox _checkBox_Bloatware, RadioButton _radioButtonUserUAC_Ativar, RadioButton _radioButtonCleanPageFile_Ativar,
            RadioButton _radioButtonradioButtonConnectionRDP_Ativar
            )
        {
            if (_instance == null)
            {
                _instance = new WinApp_Mananger
                {
                    checkBox_UserUAC = _checkBox_UserUAC,
                    checkBox_CleanTask = _checkBox_CleanTask,
                    checkBox_CleanTrash = _checkBox_CleanTrash,
                    checkBox_CleanProcess = _checkBox_CleanProcess,
                    checkBox_CleanTemp = _checkBox_CleanTemp,
                    checkBox_CleanWindowsUpdate = _checkBox_CleanWindowsUpdate,
                    checkBox_CleanGoogle = _checkBox_CleanGoogle,
                    checkBox_BackupRegistrysRun = _checkBox_BackupRegistrysRun,
                    checkBox_CleanPageFile = _checkBox_CleanPageFile,
                    checkBox_DriversBackup = _checkBox_DriversBackup,
                    checkBox_DeleteRegistry = _checkBox_DeleteRegistry,
                    checkBox_Usuario = _checkBox_Usuario,
                    checkBox_CleanPrefetch = _checkBox_CleanPrefetch,
                    checkBox_BackupBCD = _checkBox_BackupBCD,
                    checkBox_RestorePoint = _checkBox_RestorePoint,
                    checkBox_ConnectionRDP = _checkBox_ConnectionRDP,
                    checkBox_Bloatware = _checkBox_Bloatware,
                    radioButtonUserUAC_Ativar = _radioButtonUserUAC_Ativar,
                    radioButtonCleanPageFile_Ativar = _radioButtonCleanPageFile_Ativar,
                    radioButtonradioButtonConnectionRDP_Ativar = _radioButtonradioButtonConnectionRDP_Ativar
                };
            }
        }

        #endregion
        
        // Acesso à instância
        public static WinApp_Mananger Instance
        {
            get
            {
                if (_instance == null)
                    throw new Exception("WinGlobal_UIService não foi inicializado. Chame Initialize() antes de usar.");

                return _instance;
            }
        }


        #region notificacao de segurança ao usuario UAC

        public async Task UserUAC()
        {
            WinUserUAC_Mananger UAC_Mananger = new WinUserUAC_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await WinGlobal_UIService.Instance.UpdateIfonUI("Segurança do Usuario UAC", checkBox_UserUAC, Resources.UserUAC_Black, "Notificações ao Usuário (UAC):\r\nGerencia os alertas do Controle de Conta de Usuário (UAC), que ajudam a proteger o sistema contra alterações não autorizadas.");
            await Task.Delay(800);

            // Executa a função assíncrona sem bloquear a UI
            await UAC_Mananger.Mananger(radioButtonUserUAC_Ativar.Checked);
            await Task.Delay(1000);

            checkBox_UserUAC.Font = new Font(checkBox_UserUAC.Font.FontFamily, checkBox_UserUAC.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Limpar Agenda de Tarefas

        public async Task CleanTask()
        {
            WinTask_Mananger Task_Mananger = new WinTask_Mananger();

            // Atualiza a UI antes da execução assíncrona            
            await WinGlobal_UIService.Instance.UpdateIfonUI("Clean Task", checkBox_CleanTask, Resources.CleanTask_Black, "Limpar Tarefas Agendadas:\n\rRemove tarefas desnecessárias programadas no sistema, melhorando o desempenho.");

            await Task.Delay(800);
            await Task.Run(() => Task_Mananger.Mananger(), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread

            await Task.Delay(1000);
            checkBox_CleanTask.Font = new Font(checkBox_CleanTask.Font.FontFamily, checkBox_CleanTask.Font.Size, FontStyle.Strikeout);

        }

        #endregion

        #region limpar Lixeira

        public async Task CleanTrash()
        {
            WinTrash_Mananger Trash_Mananger = new WinTrash_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await WinGlobal_UIService.Instance.UpdateIfonUI("Clean Trash", checkBox_CleanTrash, Resources.CleanTrash_Black, "Limpar Lixeira:\n\rEsvazia arquivos excluídos permanentemente para liberar espaço no disco.");
            await Task.Delay(800);

            await Task.Run(() => Trash_Mananger.Mananger(), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            checkBox_CleanTrash.Font = new Font(checkBox_CleanTrash.Font.FontFamily, checkBox_CleanTrash.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Limpa Serviços

        public async Task CleanProcess()
        {
            WinService_Mananger Service_Mananger = new WinService_Mananger();
            WinService_List Service_List = new WinService_List();

            // Atualiza a UI antes da execução assíncrona
            await WinGlobal_UIService.Instance.UpdateIfonUI("Clean Process", checkBox_CleanProcess, Resources.CleanProcess_Black, "Desativar Processos:\n\rEncerra processos desnecessários em execução para reduzir o consumo de memória e CPU.");
            await Task.Delay(800);

            await Task.Run(() => Service_Mananger.Mananger(Service_List.All, WinGlobal_UIService.Instance.ValueUniProgressBar, false), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            // Atualiza a UI após a execução assíncrona  
            checkBox_CleanProcess.Font = new Font(checkBox_CleanProcess.Font.FontFamily, checkBox_CleanProcess.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Limpa a pasta Temp

        public async Task CleanTemp()
        {
            WinDirectory_Mananger Directory_Mananger = new WinDirectory_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await WinGlobal_UIService.Instance.UpdateIfonUI("Clean Temp", checkBox_CleanTemp, Resources.CleanDirectorry_Black, "Limpar Pasta %Temp%:\n\rApaga arquivos temporários do sistema e dos aplicativos para liberar espaço.");
            await Task.Delay(800);

            string DiretorioPasta = @"C:\Users\" + Environment.UserName.ToString() + @"\AppData\Local\Temp";
            await Task.Run(() => Directory_Mananger.Mananger(DiretorioPasta, "%Temp%", WinGlobal_UIService.Instance.ValueUniProgressBar), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            // Atualiza a UI após a execução assíncrona
            checkBox_CleanTemp.Font = new Font(checkBox_CleanTemp.Font.FontFamily, checkBox_CleanTemp.Font.Size, FontStyle.Strikeout);            
        }

        #endregion

        #region Desativar o Windows Update

        public async Task CleanWindowsUpdate()
        {
            WinDirectory_Mananger Directory_Mananger = new WinDirectory_Mananger();
            WinService_Mananger Service_Mananger = new WinService_Mananger();
            WinService_List Service_List = new WinService_List();

            // Atualiza a UI antes da execução assíncrona
            await WinGlobal_UIService.Instance.UpdateIfonUI("Clean Windows", checkBox_CleanWindowsUpdate, Resources.CleanTask_Black, "Desativar Update Windows:\n\rRemove atualizações antigas e corrompidas para evitar erros e liberar espaço e Desativa o serviço de Update");
            await Task.Delay(800);

            string DiretorioPasta = @"C:\Windows\SoftwareDistribution\Download";
            await Task.Run(() => Directory_Mananger.Mananger(DiretorioPasta, "Windows Update", WinGlobal_UIService.Instance.ValueUniProgressBar / 2), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread
            await Task.Run(() => Service_Mananger.Mananger(Service_List.WindowsUpdate, WinGlobal_UIService.Instance.ValueUniProgressBar / 2, true), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            // Atualiza a UI após a execução assíncrona
            checkBox_CleanWindowsUpdate.Font = new Font(checkBox_CleanWindowsUpdate.Font.FontFamily, checkBox_CleanWindowsUpdate.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Limpra Google Update

        public async Task CleanGoogle()
        {
            WinDirectory_Mananger Directory_Mananger = new WinDirectory_Mananger();
            WinService_Mananger Service_Mananger = new WinService_Mananger();
            WinService_List Service_List = new WinService_List();

            // Atualiza a UI antes da execução assíncrona
            await WinGlobal_UIService.Instance.UpdateIfonUI("Clean Google", checkBox_CleanGoogle, Resources.CleanGoogle_Black, "Limpra Google Update:\n\rApaga caches e dados temporários do navegador para melhorar o desempenho.");
            await Task.Delay(800);

            await Task.Run(() => Service_Mananger.Mananger(Service_List.Google, WinGlobal_UIService.Instance.ValueUniProgressBar / 2, false), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            string DiretorioPasta = @"C:\Program Files (x86)\Google\Update";
            await Task.Run(() => Directory_Mananger.Mananger(DiretorioPasta, "Google Update", WinGlobal_UIService.Instance.ValueUniProgressBar / 2), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            // Atualiza a UI após a execução assíncrona
            checkBox_CleanGoogle.Font = new Font(checkBox_CleanGoogle.Font.FontFamily, checkBox_CleanGoogle.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Backup Registro

        public async Task BackupRegistrysRun()
        {
            WinRegistryBackup_Mananger RegistryBackup_Mananger = new WinRegistryBackup_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await WinGlobal_UIService.Instance.UpdateIfonUI("Backup Registrys Run", checkBox_BackupRegistrysRun, Resources.BackupRegistrys_Black, "Backup Registro:\r\nSalva uma cópia dos registro, para facilitar a restauração em caso de problemas.");
            await Task.Delay(800);

            await Task.Run(() => RegistryBackup_Mananger.Mananger(), WinGlobal_UIService.Instance.token);
            await Task.Delay(1000);

            checkBox_BackupRegistrysRun.Font = new Font(checkBox_BackupRegistrysRun.Font.FontFamily, checkBox_BackupRegistrysRun.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Limpar PageFile.sys

        public async Task CleanPageFile()
        {
            WinPageFile_Mananger PageFile_Mananger = new WinPageFile_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await WinGlobal_UIService.Instance.UpdateIfonUI("Clean PageFile", checkBox_CleanPageFile, Resources.CleanPageFile_Black, "Pagefile.sys:\n\rApaga o arquivo de paginação do Windows ao desligar, garantindo segurança e desempenho.");
            await Task.Delay(800);

            // Executa a função assíncrona sem bloquear a UI
            await Task.Run(() => PageFile_Mananger.Mananger(radioButtonCleanPageFile_Ativar.Checked), WinGlobal_UIService.Instance.token);
            await Task.Delay(1000);

            checkBox_CleanPageFile.Font = new Font(checkBox_CleanPageFile.Font.FontFamily, checkBox_CleanPageFile.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Backup Drives

        public async Task DriversBackup()
        {
            WinBackupDriver_Mananger BackupDriver_Mananger = new WinBackupDriver_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await WinGlobal_UIService.Instance.UpdateIfonUI("Drivers Backup", checkBox_DriversBackup, Resources.BackupDriver_Black, "Backup Drivers:\n\rSalva uma cópia dos drivers instalados para facilitar a restauração em caso de problemas.");
            await Task.Delay(800);

            await Task.Run(() => BackupDriver_Mananger.Mananger(), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            // Atualiza a UI após a execução assíncrona  
            checkBox_DriversBackup.Font = new Font(checkBox_DriversBackup.Font.FontFamily, checkBox_DriversBackup.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Limpar Registro Run

        public async Task DeleteRegistry()
        {
            WinRegistryBin_Mananger _ClassCleanRegistry = new WinRegistryBin_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await WinGlobal_UIService.Instance.UpdateIfonUI("Delete Registry", checkBox_DeleteRegistry, Resources.CleanRegistry_Black, "Limpar Registro:\n\rRemove entradas inválidas do registro, ajudando na estabilidade do sistema.");
            await Task.Delay(800);

            await Task.Run(() => _ClassCleanRegistry.DeleteRegistry(), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            checkBox_DeleteRegistry.Font = new Font(checkBox_DeleteRegistry.Font.FontFamily, checkBox_DeleteRegistry.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Usuario Suporte

        public async Task UsuarioSuporte()
        {
            WinUser_Mananger User_Mananger = new WinUser_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await WinGlobal_UIService.Instance.UpdateIfonUI($"Manutenção de usuario {WinGlobal_UIService.Instance.UserName}", checkBox_Usuario, Resources.UpdateUser_Black, $"Usuario {WinGlobal_UIService.Instance.UserName}:\r\nConfiguração do usuário \"{WinGlobal_UIService.Instance.UserName}\" para eventuais manutenção preventiva das estações gerenciados pela empresa. ");
            await Task.Delay(800);

            await Task.Run(() => User_Mananger.Mananger(), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            checkBox_Usuario.Font = new Font(checkBox_Usuario.Font.FontFamily, checkBox_Usuario.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Limpar Prefetch

        public async Task CleanPrefetch()
        {
            WinDirectory_Mananger Directory_Mananger = new WinDirectory_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await WinGlobal_UIService.Instance.UpdateIfonUI("Clean Prefetch", checkBox_CleanPrefetch, Resources.ClearPrefetch_Black, "Limpar Prefetch:\r\nExclui arquivos de pré-carregamento do sistema para otimizar o tempo de inicialização.");
            await Task.Delay(800);

            // Executa a função assíncrona sem bloquear a UI
            string DiretorioPasta = @"C:\Windows\Prefetch";
            await Task.Run(() => Directory_Mananger.Mananger(DiretorioPasta, "Prefetch", WinGlobal_UIService.Instance.ValueUniProgressBar), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            // Atualiza a UI após a execução assíncrona    
            checkBox_CleanPrefetch.Font = new Font(checkBox_CleanPrefetch.Font.FontFamily, checkBox_CleanPrefetch.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Boot e BCD

        public async Task BackupBCD()
        {
            WinBackupBCD_Mananger BackupBCD_Mananger = new WinBackupBCD_Mananger();

            // Atualiza a UI antes da execução assíncrona
            await WinGlobal_UIService.Instance.UpdateIfonUI("Backup BCD", checkBox_BackupBCD, Resources.BackupBootBCD_Black, "Backup BootBCD:\r\nFaz cópia de segurança da configuração de inicialização do Windows para evitar falhas no boot.");
            await Task.Delay(800);

            await Task.Run(() => BackupBCD_Mananger.Mananger(), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            checkBox_BackupBCD.Font = new Font(checkBox_BackupBCD.Font.FontFamily, checkBox_BackupBCD.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Proteção de Propriedades do Sistema

        public async Task CreateSystemPointOld()
        {
            await WinGlobal_UIService.Instance.UpdateIfonUI("Proteção do Sistema", checkBox_RestorePoint, Resources.restore_Black, checkBox_RestorePoint.Text + "Ponto de Restauração\n\rpermite reverter o sistema para um estado anterior, revertendo configurações, programas e arquivos do sistema");
            await Task.Delay(800);

            await CreateSystemPoint("Old");
            await Task.Delay(1000);

            checkBox_RestorePoint.Font = new Font(checkBox_DriversBackup.Font.FontFamily, checkBox_DriversBackup.Font.Size);
        }

        public async Task CreateSystemPointNew()
        {
            await WinGlobal_UIService.Instance.UpdateIfonUI("Proteção do Sistema", checkBox_RestorePoint, Resources.restore_Black, checkBox_RestorePoint.Text + "Ponto de Restauração\n\rpermite reverter o sistema para um estado anterior, revertendo configurações, programas e arquivos do sistema");
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
                await _Class_EnableProtection.EnableProtection(WinGlobal_UIService.Instance.ValueUniProgressBar / 3);
                await Task.Delay(1000);
            }

            // Gera uma Descrição para o Ponto de Restauração
            WinRestorePoint_PointName _PointName = new WinRestorePoint_PointName();
            string NamePoint = _PointName.GetName(State);
            await Task.Delay(200);

            //2° Cria Ponto de Restauração
            WinRestorePoint_Create _PointCreate = new WinRestorePoint_Create();
            await Task.Run(() => _PointCreate.CreatePoint(NamePoint, WinGlobal_UIService.Instance.ValueUniProgressBar / 3), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            //3° Procura pelo Ponto de Restauração recém-criado
            WinRestorePoint_PointSearch _PointSearch = new WinRestorePoint_PointSearch();
            await Task.Run(() => _PointSearch.PointSearch(NamePoint, WinGlobal_UIService.Instance.ValueUniProgressBar / 3), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread                 
        }

        #endregion

        #region Acesso Remoto RDP

        public async Task RemoteRDP()
        {
            WinRemoteRDP_Mananger RemoteRDP_Mananger = new WinRemoteRDP_Mananger();    
            // Atualiza a UI antes da execução assíncrona
            WinGlobal_UIService.Instance.UpdateIfonUI("Remote Desktop Connection", checkBox_ConnectionRDP, Resources.Remoto_Black, "Remote Desktop Connection RDP\r\nPermite outros computadores na mesma rede se conecta via TS em sua maquina para manutenções remota");
            await Task.Delay(800);

            await Task.Run(() => RemoteRDP_Mananger.Mananger(radioButtonradioButtonConnectionRDP_Ativar.Checked), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);
                        
            // Atualiza a UI após a execução assíncrona
            checkBox_BackupBCD.Font = new Font(checkBox_BackupBCD.Font.FontFamily, checkBox_BackupBCD.Font.Size, FontStyle.Strikeout);
        }

        #endregion

        #region Bloatware


        public async Task RenoveBloatware()
        {
            WinBloatware_Mananger RemoveBloatware = new WinBloatware_Mananger();
            await WinGlobal_UIService.Instance.UpdateIfonUI("Remover Bloatware", checkBox_Bloatware, Resources.Store_Black, "Bloatware:\r\nSoftwares pré-instalados que nem sempre são úteis para o usuário, como alguns jogos ou versões de teste de programas pagos.");
            await Task.Delay(800);

            await Task.Run(() => RemoveBloatware.Mananger(), WinGlobal_UIService.Instance.token); // Executa a tarefa async em uma nova thread
            await Task.Delay(1000);

            // Atualiza a UI após a execução assíncrona    
            checkBox_Bloatware.Font = new Font(checkBox_Bloatware.Font.FontFamily, checkBox_Bloatware.Font.Size, FontStyle.Strikeout);
        }

        public async Task Bloatware_ExtractProgress(int valor)
        {


            int valorMaximo = (valor > 1) ? Convert.ToInt32(valor) : 100;

            float unidade = (float)WinGlobal_UIService.Instance.ValueUniProgressBar / (float)valorMaximo;

            WinGlobal_UIService.Instance.ProgressBarADD(await ValueUnitBloatware(unidade));
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


    }
}
