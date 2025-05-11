using System;
using System.Linq;
using System.Threading.Tasks;


namespace MeuSuporte
{
    internal class WinGlobal_UIService
    {
        private readonly System.Windows.Forms.TextBox _logTextBox;
        private readonly System.Windows.Forms.ProgressBar _progressBar;

        public MainForm InterfaceGUI;
        public int ValueUniProgressBar;
        public int Sucesso;
        public int Erro;

        public WinGlobal_UIService(MainForm Form_, System.Windows.Forms.TextBox logTextBox, System.Windows.Forms.ProgressBar progressBar, int _ValueUniProgressBar)
        {
            InterfaceGUI = Form_;
            _logTextBox = logTextBox;
            _progressBar = progressBar;
            ValueUniProgressBar = _ValueUniProgressBar;
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

        // Adiciona Informações na TextBox Log, sobrescrevendo a linha atual   
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
