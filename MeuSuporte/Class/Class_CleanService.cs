using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TimeoutException = System.TimeoutException;

namespace MeuSuporte
{
    internal class Class_CleanService
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

        public Class_CleanService(MainForm Form_)
        {
            _MainForm = Form_;
        }

        #region Tarefas Plublicas

        public async Task CleanProcess(string[] ListService, int ValueUniProgressBar, bool isDisableService, CancellationToken token)
        {    
            token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

            int total = ListService.Length;

            //int total = ListProcessDelete.Length;
            float valorUnidade = (float)ValueUniProgressBar / total;
            bool foundServices = false;

            foreach (string serviceName in ListService)
            {
                token.ThrowIfCancellationRequested();

                var service = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == serviceName);

                int NewValor = await ValueUnit(valorUnidade);

                if (NewValor >= 1)
                {
                    _MainForm.ProgressBarADD(NewValor);
                    await Task.Delay(20);
                }

                if (!isDisableService & service != null)
                {
                    await UninstallProcess(service);
                    foundServices = true;
                }

                if (isDisableService & service != null)
                {
                    await WaitForServiceToDisabled(service);
                    foundServices = true;
                }
            }

            if (!foundServices)
            {
                _MainForm.ProgressBarADD(ValueUniProgressBar);
                _MainForm.Log_MensagemAsync("Serviço: No listings found!", true);
                await Task.Delay(500);
            }
        }


        // Funcao que envia a porcentagem para ProgressBar
        private static float accumulator = 0f; // Variável para armazenar o valor acumulado    
        private static async Task<int> ValueUnit(float valor)
        {
            accumulator += valor;

            // Extrai a parte inteira e armazena na ProgressBar
            int parteInteira = (int)accumulator;

            if (parteInteira > 0)
            {
                accumulator -= parteInteira;
                return parteInteira;
            }
            return 0;
        }

        #endregion


        #region Funcoes internas

        //Metodo de Remove Serviço
        private async Task UninstallProcess(ServiceController service)
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

        //Metodo de Desable Serviço
        private async Task WaitForServiceToDisabled(ServiceController service)
        {
            bool serviceStopped = await WaitForServiceToStop(service); // Para o serviço   

            if (!serviceStopped)
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

        //Metodo de Stop Serviço
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
