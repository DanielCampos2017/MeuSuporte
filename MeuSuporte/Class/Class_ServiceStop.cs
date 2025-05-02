using System;
using System.ServiceProcess;
using System.Threading.Tasks;


namespace MeuSuporte
{
    internal class Class_ServiceStop
    {
        private readonly MainForm _mainForm;

        public Class_ServiceStop(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

        public async Task<bool> WaitForServiceToStop(ServiceController service)
        {
            bool isServiceStopped = false;

            try
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    await _mainForm.Log_MensagemAsync($"Serviço: {service.DisplayName} - Stopping", true);
                    await Task.Delay(500);
                    service.Stop(); // Envia o comando para parar
                    await Task.Delay(4000); // Espera 4 segundos
                }
            }
            catch (Exception ex)
            {
                await _mainForm.Log_MensagemAsync($"Serviço: {service.DisplayName} - Stop failed.", true);
                await Task.Delay(500);
                _mainForm.Erro++;
                return false;
            }

            // acompanha o encerramento do processo loop 5x
            for (int i = 0; i < 5 && service.Status == ServiceControllerStatus.Running; i++)
            {
                await Task.Delay(3000);// Espera 3 segundos
                service.Refresh(); // Atualiza o status do serviço                
            }

            await _mainForm.Log_MensagemAsync($"Serviço: {service.DisplayName} - {service.Status}.", true);
            await Task.Delay(500);

            // atribui como true a variavel isServiceStopped se o resultado for Stopped   
            return  service.Status == ServiceControllerStatus.Stopped;
        }
    }
}
