using System;
using System.Configuration.Install;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using TimeoutException = System.TimeoutException;

namespace MeuSuporte
{
    internal class Class_CleanProcess
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool ChangeServiceConfig(
            IntPtr hService,
            uint nServiceType,
            uint nStartType,
            uint nErrorControl,
            string lpBinaryPathName,
            string lpLoadOrderGroup,
            IntPtr lpdwTagId,
            [In] char[] lpDependencies,
            string lpServiceStartName,
            string lpPassword,
            string lpDisplayName);

        private const uint SERVICE_NO_CHANGE = 0xFFFFFFFF;

        private MainForm _MainForm;

        public Class_CleanProcess(MainForm Form_)
        {
            _MainForm = Form_;
        }

        #region Lista de Processos

        private readonly string[] ListProcessGoogle = {
            "edgeupdate", // serviço do google
            "edgeupdatem", // serviço do google
             "gupdatem",  // serviço do google
            "gupdate",  // serviço do google
            "GoogleChromeElevationService",  // serviço do google
            "gusvc",  // serviço do google
        };

        private readonly string[] ListProcessWindowsUpdate =
        {
            "InstallService", // Serviço de Instalação da Microsoft Store
            "wuauserv",  // Serviço de Windows Update
            "WSearch"        // Se não usa a pesquisa do Windows com frequência, pode desativar para reduzir o uso do disco.

        };

        private readonly string[] ListProcessDelete =
        {
            "AdobeUpdateService", // serviço da adobe
            "AGSService",         // serviço da adobe
            "AdobeARMservice",    // serviço da adobe
            "Microsoft Office Groove Audit Service", // serviço do Office
            "DbxSvc",   // Dropbox Service
            "SysMain",  // Pode ajudar em SSDs, mas em HDs pode ser útil.
            "UsoSvc", // Atualizar o Serviço Orchestrator    
            "XboxGipSvc", // Xbox Accessory Management Service
            "vmicvmsession", // Serviço Direto do Hyper-V PowerShell
            "vmicrdv", // Serviço de Virtualização de Área de Trabalho Remota do Hyper-V
            "UevAgentService", // Serviço de User Experience Virtualization
            "vmickvpexchange", // Serviço de Troca de Dados do Hyper-V
            "PhoneSvc", // Serviço de Telefonia
            "vmictimesync", // Serviço de Sincronização de Data/Hora do Hyper-V
            "perceptionsimulation", // Serviço de Simulação de Percepção do Windows
            "SensorService", // Serviço de Sensor rotação automática
            "XboxNetApiSvc", // Serviço de Rede Xbox Live
            "vmicheartbeat", // Serviço de Pulsação do Hyper-V
            "HvHost", // Serviço de Host HV
            "cloudidsvc", // Serviço de identidade Microsoft Cloud
            "icssvc", // Serviço de Hotspot Móvel do Windows
            "fhsvc", // Serviço de Histórico de Arquivos
            "lfsvc", // Serviço de Geolocalização
            "refsdedupsvc", // Serviço de duplicação do ReFS
            "vmicshutdown", // Serviço de Desligamento de Convidado do Hyper-V
            "WMPNetworkSvc", // Serviço de Compartilhamento de Rede do Windows Media Player
            "WbioSrvc", // Serviço de Biometria do Windows
            "Fax", // Se você não usa fax, pode desativar.           
            "SEMgrSvc", // Serviço de Gerenciador de NFC/SE e Pagamentos
            "MapsBroker", // Serviço de Mapas
            "WpnService", // Serviço de Notificações Push do Windows
            "XblAuthManager", // Serviço de Autenticação Xbox Live
            "vmicvss" // Solicitante de Cópia de Sombra de Volume do Hyper-V
        };

        #endregion


        #region Tarefas Plublicas
          
        public async Task CleanProcess(int ValueUniProgressBar, CancellationToken token)
        {
            int ValueProgress = ValueUniProgressBar / ListProcessDelete.Length;
            int totalItens = ValueUniProgressBar / ListProcessDelete.Length;

            token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

            int ValueUniBar = ValueUniProgressBar;
            int total = ListProcessDelete.Length;
            float valorUnidade = (float)ValueUniBar / total;
            float valorAcumulado = 0f;


            //filtrar os serviços que estão na lista ListProcessDelete   
            var services = ServiceController.GetServices().Where(s => ListProcessDelete.Contains(s.ServiceName));

            foreach (ServiceController service in services)
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                
                valorAcumulado += valorUnidade;
                if (valorAcumulado >= 1)
                {
                    _MainForm.ProgressBarADD(1);
                    valorAcumulado -= 1;
                    await Task.Delay(20);
                }
                await UninstallProcess(service, totalItens);
            }

            if (!services.Any())
            {
                _MainForm.ProgressBarADD(ValueUniProgressBar);
                _MainForm.Log_MensagemAsync("Serviço: No listings found!", true);
                await Task.Delay(500);
            }
        }

        public async Task CleanProcessGoogle(int ValueUniProgressBar, CancellationToken token)
        {
            int ValueProgress = ValueUniProgressBar / ListProcessGoogle.Length;
            int totalItens = ValueUniProgressBar / ListProcessGoogle.Length;

            token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

            //filtrar os serviços que estão na lista ListProcessGoogle   
            var services = ServiceController.GetServices().Where(s => ListProcessGoogle.Contains(s.ServiceName));

            foreach (ServiceController service in services)
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                await UninstallProcess(service, totalItens);
            }

            if (!services.Any())
            {
                _MainForm.ProgressBarADD(ValueUniProgressBar);
                _MainForm.Log_MensagemAsync("Serviço: Nenhum Listado encontrado !", true);
                await Task.Delay(500);
            }
        }

        public async Task WindowsUpdate(int ValueUniProgressBar, CancellationToken token)
        {
            int ValueProgress = ValueUniProgressBar / ListProcessWindowsUpdate.Length;
            int totalItens = ValueUniProgressBar / ListProcessWindowsUpdate.Length;

            token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

            //filtrar os serviços que estão na lista ListProcessWindowsUpdate   
            var services = ServiceController.GetServices().Where(s => ListProcessWindowsUpdate.Contains(s.ServiceName));

            foreach (ServiceController service in services)
            {
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                await WaitForServiceToDisabled(service, totalItens); // coloca o serviço com o status como desativado.
            }

            if (!services.Any())
            {
                _MainForm.ProgressBarADD(ValueUniProgressBar);
                _MainForm.Log_MensagemAsync("Serviço: Nenhum Listado encontrado !", true);
                await Task.Delay(500);
            }
        }

        #endregion


        #region Funcoes internas

        //Metodo de desinstalacao de processos
        private async Task UninstallProcess(ServiceController service, int totalItens)
        {
            bool serviceStopped = await WaitForServiceToStop(service);

            if (!serviceStopped)
            {
                _MainForm.Log_MensagemAsync($"Serviço: {service.DisplayName} não pode ser removido devido ainda está em execução ", true);
                await Task.Delay(500);
                return;
            }

            try
            {
                using (var serviceInstaller = new ServiceInstaller())
                {
                    serviceInstaller.Context = new InstallContext(null, null);
                    serviceInstaller.ServiceName = service.ServiceName;
                    serviceInstaller.Uninstall(null);
                    _MainForm.Log_MensagemAsync($"Serviço: {service.DisplayName} - Deleted", true);
                    await Task.Delay(500);
                    _MainForm.ProgressBarADD(totalItens);                   
                }
            }
            catch (InvalidOperationException ex)
            {
                _MainForm.Erro++;
                await _MainForm.Log_MensagemAsync($"Erro ao Tentar parar o Serviço: {service.DisplayName} \n " + ex.Message, true);
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                _MainForm.Erro++;
                await _MainForm.Log_MensagemAsync($"Erro inesperado ao desinstalar o Serviço: {service.DisplayName} \n " + ex.Message, true);
                await Task.Delay(500);
            }
        }

        // funcao de desabilitar o servico
        private async Task WaitForServiceToDisabled(ServiceController service, int totalItens)
        {
            bool ServiceStatus = await WaitForServiceToStop(service); // Para o serviço   

            if (!ServiceStatus)
            {
                _MainForm.Log_MensagemAsync($"Serviço: {service.DisplayName} não pode ser desabilitado devido ainda está em execução ", true);
                await Task.Delay(500);
                _MainForm.Erro++;
                return;
            }

            if (service.StartType != ServiceStartMode.Disabled)
            {
                try
                {
                    using (var serviceHandle = service.ServiceHandle)
                    {
                        ChangeServiceConfig(
                            serviceHandle.DangerousGetHandle(),
                            SERVICE_NO_CHANGE,
                            (uint)ServiceStartMode.Disabled,
                            SERVICE_NO_CHANGE,
                            null,
                            null,
                            IntPtr.Zero,
                            null,
                            null,
                            null,
                            null);
                    }

                    _MainForm.Log_MensagemAsync($"Serviço:  {service.DisplayName} - Disabled", true);
                    await Task.Delay(500);
                    _MainForm.ProgressBarADD(totalItens);
                    _MainForm.Sucesso++;
                }
                catch (Exception ex)
                {
                    _MainForm.Log_MensagemAsync($"Erro ao Tentar mudar o Modo de Inicialização do Serviço: {service.DisplayName} " + ex.Message, true);
                    await Task.Delay(500);
                    _MainForm.Erro++;
                }
            }

        }

        private async Task<bool> WaitForServiceToStop(ServiceController service)
        {
            bool isServiceStopped = false;

            try
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    await _MainForm.Log_MensagemAsync($"Serviço: {service.DisplayName} - Stopping", true);
                    await Task.Delay(500);
                    service.Stop(); // Envia o comando para parar
                    await Task.Delay(4000); // Espera 4 segundos
                }
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is TimeoutException)
            {
                await _MainForm.Log_MensagemAsync($"Serviço: {service.DisplayName} - Stop failed.", true);
                await Task.Delay(500);
                _MainForm.Erro++;
                return false;
            }

            // acompanha o encerramento do processo loop 5x
            for (int i = 0; i < 5 && service.Status == ServiceControllerStatus.Running; i++)
            {
                await Task.Delay(3000);// Espera 3 segundos
                service.Refresh(); // Atualiza o status do serviço                
            }

            await _MainForm.Log_MensagemAsync($"Serviço: {service.DisplayName} - {service.Status}.", true);
            await Task.Delay(500);

            // atribui como true a variavel isServiceStopped se o resultado for Stopped
            isServiceStopped = service.Status == ServiceControllerStatus.Stopped; 
            await Task.Delay(500);

            return isServiceStopped;
        }

        #endregion

    }
}
