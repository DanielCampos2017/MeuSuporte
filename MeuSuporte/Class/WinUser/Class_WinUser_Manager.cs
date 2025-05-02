using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class Class_WinUser_Manager
    {
        private MainForm _MainForm;
        Class_WinUser_Account WinUser_Account;
        Class_Password_Public Password_Public;
       // Class_Password_Private Password_Private;
        Class_WinUser_AccountUpdate WinUser_AccountUpdate;
        Class_WinUser_AccountCreate WinUser_AccountCreate;

        public Class_WinUser_Manager(MainForm Form_)
        {
            _MainForm = Form_;
            WinUser_Account = new Class_WinUser_Account();
            Password_Public = new Class_Password_Public();
          //  Password_Private = new Class_Password_Private();
            WinUser_AccountUpdate = new Class_WinUser_AccountUpdate(_MainForm);
            WinUser_AccountCreate = new Class_WinUser_AccountCreate(_MainForm);
        }

        public async Task Manager(CancellationToken token, int ValueUniProgressBar)
        {
            string NameUser = Password_Public.GetUser();
            string PasswordUser = Password_Public.GetPassword();
            //NameUser = Password_Private.GetUser();
            //PasswordUser = Password_Private.GetPassword();

            // verifica se existe usuario
            if (await WinUser_Account.IsEnabled(NameUser))
            {
                await WinUser_AccountUpdate.Update(token, ValueUniProgressBar, NameUser, PasswordUser);
                return;
            }                                  
           
            await WinUser_AccountCreate.Create(token, ValueUniProgressBar, NameUser, PasswordUser);
        }
    }
}
