using System;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_PointCreate
    {
        private MainForm _MainForm;

        public Class_PointCreate(MainForm Form_)
        {
            _MainForm = Form_;
        }

        public async Task CreatePoint(string description, int ValueUniProgressBar, CancellationToken token)
        {
            await Task.Run(async () =>
            {
                try
                {
                    token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                    ManagementScope oScope = new ManagementScope("\\\\localhost\\root\\default");
                    ManagementPath oPath = new ManagementPath("SystemRestore");
                    ObjectGetOptions oGetOp = new ObjectGetOptions();
                    ManagementClass oProcess = new ManagementClass(oScope, oPath, oGetOp);

                    ManagementBaseObject oInParams = oProcess.GetMethodParameters("CreateRestorePoint");
                    oInParams["Description"] = description;
                    oInParams["RestorePointType"] = 12; // 12 - funcional
                    oInParams["EventType"] = 100;

                    //Define o tipo de ponto de restauração. Os valores possíveis são:
                    // 0 – Um ponto de restauração indefinido.
                    // 10 – Instalação do sistema operacional.
                    // 12 – Instalação de aplicativo (padrão para criar manualmente).
                    // 13 – Modificação do sistema.

                    ManagementBaseObject oOutParams = oProcess.InvokeMethod("CreateRestorePoint", oInParams, null);

                    int returnValue = Convert.ToInt32(oOutParams["ReturnValue"]);
                    if (returnValue != 0)
                    {
                        await _MainForm.Log_MensagemAsync("Erro ao criar ponto de restauração. Código: " + returnValue, true);
                        _MainForm.Erro++;
                    }
                    else
                    {
                        _MainForm.Sucesso++;
                        _MainForm.ProgressBarADD(ValueUniProgressBar);
                        await _MainForm.Log_MensagemAsync($"Ponto de restauração [{description}] Criado com sucesso.", true);
                    }
                }     
                catch (Exception ex)
                {
                    _MainForm.Erro++;
                    await _MainForm.Log_MensagemAsync($"Erro ao criar ponto de restauração Código:" + ex.Message, true);
                }
            });
        }
    }
}
