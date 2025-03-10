using System;
using System.Configuration.Install;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace TaskScheduler
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
        private const uint SERVICE_QUERY_CONFIG = 0x00000001;
        private const uint SERVICE_CHANGE_CONFIG = 0x00000002;
        private const uint SC_MANAGER_ALL_ACCESS = 0x000F003F;

        private MainForm _MainForm;

        public Class_CleanProcess(MainForm formPreventiva)
        {
            _MainForm = formPreventiva;
        }

        private readonly string[] ListProcessDelete =
        {
    "AdobeUpdateService", // serviço da adobe
    "AGSService",         // serviço da adobe
    "AdobeARMservice",    // serviço da adobe
    "Microsoft Office Groove Audit Service", // serviço do Office
    "edgeupdate", // serviço do google
    "edgeupdatem", // serviço do google
    "gupdatem",  // serviço do google
    "gupdate",  // serviço do google
    "GoogleChromeElevationService",  // serviço do google
    "gusvc",  // serviço do google
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
     "XblAuthManager" // Serviço de Autenticação Xbox Live
};


        private readonly string[] ListProcessDisable =
        {
            "InstallService", // Serviço de Instalação da Microsoft Store
            "svsvc",         // Serviço de Verificador de Ponto
            "wuauserv",  // Serviço de Windows Update
            "WSearch"        // Se não usa a pesquisa do Windows com frequência, pode desativar para reduzir o uso do disco.

        };

        private void UninstallProcess2(string Nome, string Titulo)
        {
            try
            {
                ServiceInstaller ServiceInstallerObj = new ServiceInstaller();
                InstallContext Context = new InstallContext("<<log file path>>", null);
                ServiceInstallerObj.Context = Context;
                ServiceInstallerObj.ServiceName = Nome;
                ServiceInstallerObj.Uninstall(null); // Apaga o serviço
                _MainForm.Log_Mensagem("Serviço: ", Titulo + " Apagado !");
            }
            catch (Exception ex)
            {
                _MainForm.Log_Mensagem("Erro ao Apagar Serviço: ", Titulo);
            }
        }

        private void UninstallProcess(string Nome, string Titulo)
        {
            try
            {
                ServiceInstaller ServiceInstallerObj = new ServiceInstaller();
                InstallContext Context = new InstallContext(null, new string[] { "/logFile=uninstall.log" });
                ServiceInstallerObj.Context = Context;
                ServiceInstallerObj.ServiceName = Nome;

                // Definir credenciais de administrador
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);

                if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
                {
                    ServiceInstallerObj.Uninstall(null); // Apaga o serviço
                    _MainForm.Log_Mensagem("Serviço: ", Titulo + " Apagado !");
                }
                else
                {
                    _MainForm.Log_Mensagem("Erro ao Apagar Serviço: ", Titulo + " - Permissão insuficiente. Execute como Administrador.");
                }
            }
            catch (Exception ex)
            {
                _MainForm.Log_Mensagem("Erro ao Apagar Serviço: ", Titulo + " - " + ex.Message);
            }
        }

        private void MudaStartMode(ServiceController service, ServiceStartMode mode)
        {
            try
            {
                using (var serviceHandle = service.ServiceHandle)
                {
                    ChangeServiceConfig(
                        serviceHandle.DangerousGetHandle(),
                        SERVICE_NO_CHANGE,
                        (uint)mode,
                        SERVICE_NO_CHANGE,
                        null,
                        null,
                        IntPtr.Zero,
                        null,
                        null,
                        null,
                        null);

                    _MainForm.Log_Mensagem("Serviço: ", service.DisplayName + " Desativado");
                }
            }
            catch (Exception ex)
            {
                _MainForm.Log_Mensagem("Erro ao Tentar mudar o Modo de Inicialização do Serviço: ", service.DisplayName + " - " + ex.Message);
            }


        }

        // função de parar serviço
        private async Task WaitForServiceToStop(ServiceController service)
        {
            service.Stop(); // Envia o comando para parar

            while (service.Status == ServiceControllerStatus.Running)
            {
                await Task.Delay(500); // Espera sem bloquear a thread
                service.Refresh(); // Atualiza o status do serviço
            }
        }


        public async Task CleanProcess()
        {
            if (_MainForm.AbortExecution)
            {
                return;
            }
            bool ProcessFound = false;
            ServiceController[] services = ServiceController.GetServices();

            // Percorre a lista de serviços e verfica se existe algum serviço na lista para deletar
            foreach (ServiceController service in services)
            {
                foreach (string name in ListProcessDelete)
                {
                    if (_MainForm.AbortExecution)
                    {
                        _MainForm.Log_Mensagem("Serviço: ", "processo canselado ");
                        break;
                    }

                    if (service.ServiceName == name)
                    {
                        ProcessFound = true;

                        if (service.Status == ServiceControllerStatus.Running)
                        {
                            try
                            {
                                _MainForm.Log_Mensagem("Serviço: ", service.DisplayName + " Parando...");
                                await WaitForServiceToStop(service);       // Para o serviço
                                _MainForm.Log_Mensagem("Serviço: ", service.DisplayName + " Parado.");
                                _MainForm.Sucesso++;
                            }
                            catch (Exception e)
                            {
                                _MainForm.Erro++;
                                _MainForm.Log_Mensagem("Erro ao Tentar parar o Serviço: ", service.DisplayName + " \n " + e.Message);
                                continue; // Impede a tentativa de exclusão caso o serviço não possa ser parado
                            }
                        }
                        UninstallProcess(name, service.DisplayName);
                    }
                }
            }

            if (!ProcessFound)
            {
                _MainForm.Log_Mensagem("Serviço: ", "Nenhum Listado encontrado !");
            }
        }

        public async Task WindowsUpdate()
        {
            if (_MainForm.AbortExecution)
            {
                return;
            }
            bool ProcessFound = false;
            ServiceController[] services = ServiceController.GetServices();

            // Percorre a lista de serviços para desativar
            foreach (ServiceController service in services)
            {
                foreach (string nome in ListProcessDisable)
                {
                    if (service.ServiceName == nome)
                    {
                        ProcessFound = true;

                        MudaStartMode(service, ServiceStartMode.Disabled); // coloca o serviço com o status como desativado. 

                        if (service.Status == ServiceControllerStatus.Running)
                        {
                            try
                            {
                                _MainForm.Log_Mensagem("Serviço: ", service.DisplayName + " Parando...");
                                await WaitForServiceToStop(service);    // Para o serviço
                                _MainForm.Sucesso++;
                            }
                            catch (Exception e)
                            {
                                _MainForm.Erro++;
                                _MainForm.Log_Mensagem("Erro ao Tentar parar o Serviço: ", service.DisplayName + " \n " + e.Message);
                            }
                        }
                    }
                }
            }

            if (!ProcessFound)
            {
                _MainForm.Log_Mensagem("Serviço: ", "Nenhum Listado encontrado !");
            }
        }




    }
}
