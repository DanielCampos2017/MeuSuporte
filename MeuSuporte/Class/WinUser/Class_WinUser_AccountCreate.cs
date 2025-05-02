using System.DirectoryServices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_WinUser_AccountCreate
    {
        private MainForm _MainForm;
        Class_WinUser_GroupAdministrators WinUser_GroupAdministrators;
        Class_WinUser_GroupRemote WinUser_GroupRemote;
       
        public Class_WinUser_AccountCreate(MainForm Form_)
        {
            _MainForm = Form_;
            WinUser_GroupAdministrators = new Class_WinUser_GroupAdministrators();
            WinUser_GroupRemote = new Class_WinUser_GroupRemote();
        }

        public async Task Create(CancellationToken token, int ValueUniProgressBar, string NameUser, string PasswordUser )
        {
            try
            {
                _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
                token.ThrowIfCancellationRequested(); // Checa se o cancelamento foi solicitado antes de começar

                // cria usuario
                DirectoryEntry EntradaDiretorioGrupos = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
                DirectoryEntry EntradaDiretorioUsuarios = EntradaDiretorioGrupos.Children.Add(NameUser, "user");
                EntradaDiretorioUsuarios.Invoke("SetPassword", new object[] { PasswordUser });
                EntradaDiretorioUsuarios.CommitChanges();

                // atribui os grupos para o usuario
                await WinUser_GroupRemote.Add(EntradaDiretorioUsuarios);
                await WinUser_GroupAdministrators.Add(EntradaDiretorioUsuarios);

                _MainForm.ProgressBarADD(ValueUniProgressBar / 2);
                _MainForm.Sucesso++;
                _MainForm.Log_MensagemAsync($"Conta Local de Suporte Tecnico criado !", true);
                _MainForm.Log_MensagemAsync($"Usuario: {NameUser}", true);
                _MainForm.Log_MensagemAsync($"Senha: {PasswordUser}", true);
            }
            catch (Exception ex)
            {
                _MainForm.Erro++;
                _MainForm.Log_MensagemAsync($"Erro: {ex.Message}", true);
            }
        }
    }
}
