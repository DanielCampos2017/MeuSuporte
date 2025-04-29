using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeuSuporte.Class.SystemProtection
{
    internal class Class_PointSearch
    {
        private MainForm _MainForm;

        public Class_PointSearch(MainForm Form_)
        {
            _MainForm = Form_;
        }

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
                Class_SystemRestorePoints systemRestorePoints = new Class_SystemRestorePoints();
                bool pontoValido = false;

                // Gera as descrições esperadas
                foreach (string tipo in TypePoint)
                {
                    token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar
                    ListaNamePoint.Add($"{tipo}: {NamePoint}");
                }

                // Procura se já existe um ponto com essas descrições
                foreach (Class_PointItem sri in systemRestorePoints)
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
                _MainForm.ProgressBarADD(ValueUniProgressBar);
                await _MainForm.Log_MensagemAsync($"Ponto [{NamePoint}] validado com sucesso.", true);
            }
            else
            {
                _MainForm.Erro++;
                await _MainForm.Log_MensagemAsync($"Ponto [{NamePoint}] não validado.", true);                
            }
        }

    }
}
