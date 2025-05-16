using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeuSuporte
{
    internal class WinGlobal_UIService2
    {
        private static WinGlobal_UIService2 _instance;
        private TextBox _logTextBox;
        private ProgressBar _progressBar;

        public MainForm InterfaceGUI;
        public int ValueUniProgressBar;
        public int Sucesso;
        public int Erro;

        private WinGlobal_UIService2() { }

        // Inicialização (chame no MainForm no início)
        public static void Initialize(MainForm form, TextBox logTextBox, ProgressBar progressBar, int valueUniProgressBar)
        {
            if (_instance == null)
            {
                _instance = new WinGlobal_UIService2
                {
                    InterfaceGUI = form,
                    _logTextBox = logTextBox,
                    _progressBar = progressBar,
                    ValueUniProgressBar = valueUniProgressBar
                };
            }
        }

        // Acesso à instância
        public static WinGlobal_UIService2 Instance
        {
            get
            {
                if (_instance == null)
                    throw new Exception("WinGlobal_UIService não foi inicializado. Chame Initialize() antes de usar.");

                return _instance;
            }
        }

        public void ProgressBarADD(int valor)
        {
            if (_progressBar.InvokeRequired)
            {
                _progressBar.Invoke(new Action(() => ProgressBarADD(valor)));
                return;
            }

            int _max = _progressBar.Value + valor;

            if (_max < 100)
            {
                _progressBar.Value += valor;
            }
            else
            {
                _progressBar.Value = 100;
            }
        }

        public async Task Log_MensagemAsync(string mensagem, bool pularLinha)
        {
            if (_logTextBox.InvokeRequired)
            {
                await Task.Run(() =>
                {
                    _logTextBox.Invoke(new Action(() =>
                    {
                        _logTextBox.AppendText(mensagem);
                        if (pularLinha)
                            _logTextBox.AppendText(Environment.NewLine);
                    }));
                });
            }
            else
            {
                _logTextBox.AppendText(mensagem);
                if (pularLinha)
                    _logTextBox.AppendText(Environment.NewLine);
            }
        }

        public async Task Log_MensagemAsyncSobrescrever(string mensagem)
        {
            if (_logTextBox.InvokeRequired)
            {
                await Task.Run(() =>
                {
                    _logTextBox.Invoke(new Action(() =>
                    {
                        string[] linhas = _logTextBox.Text.Split('\n');
                        _logTextBox.Text = string.Join("\n", linhas.Take(linhas.Length - 1).Concat(new[] { mensagem }));
                    }));
                });
            }
            else
            {
                string[] linhas = _logTextBox.Text.Split('\n');
                _logTextBox.Text = string.Join("\n", linhas.Take(linhas.Length - 1).Concat(new[] { mensagem }));
            }
        }
    }
}
