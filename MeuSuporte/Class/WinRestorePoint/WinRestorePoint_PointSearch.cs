using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinRestorePoint_PointSearch
    {
        private readonly string[] TypePoint = {
            "Install", "Modified", "Uninstall", "Driver Install",
            "Checkpoint", "Critical Update", "Security Update",
            "Manual", "System"
        };

        public async Task<bool> IsPointCreatedAsync(string NamePoint, CancellationToken token)
        {
            return await Task.Run(() =>
            {
                List<string> ListaNamePoint = new List<string>();
                WinRestorePoint_SystemPoints systemRestorePoints = new WinRestorePoint_SystemPoints();
                bool pontoValido = false;

                // Gera as descrições esperadas
                foreach (string tipo in TypePoint)
                {
                    token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                    ListaNamePoint.Add($"{tipo}: {NamePoint}");
                }

                // Procura se já existe um ponto com essas descrições
                foreach (WinRestorePoint_Item sri in systemRestorePoints)
                {
                    token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                    foreach (string descricao in ListaNamePoint)
                    {
                        token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                        if (sri.Description == descricao)
                        {
                            pontoValido = true;
                            break;
                        }
                    }

                    if (pontoValido)
                    {
                        break;
                    }
                }
                return pontoValido;
            });
        }

        public async Task PointSearch(string NamePoint, int ValueUniProgressBar, CancellationToken token)
        {
            // Procura pelo Ponto de Restauração recém-criado
            if (await Task.Run(() => IsPointCreatedAsync(NamePoint, token)))
            {
                WinGlobal_UIService2.Instance.ProgressBarADD(ValueUniProgressBar);
                await WinGlobal_UIService2.Instance.Log_MensagemAsync($"Ponto [{NamePoint}] validado com sucesso.", true);
            }
            else
            {
                WinGlobal_UIService2.Instance.Erro++;
                await WinGlobal_UIService2.Instance.Log_MensagemAsync($"Ponto [{NamePoint}] não validado.", true);                
            }
        }

    }
}
