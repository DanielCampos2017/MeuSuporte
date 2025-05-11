using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinUser_Mananger
    {
        private readonly WinGlobal_UIService UIService;
        WinUser_Account WinUser_Account;
        Password_Public Password_Public;
       // Password_Private Password_Private;
        WinUser_AccountUpdate WinUser_AccountUpdate;
        WinUser_AccountCreate WinUser_AccountCreate;

        public WinUser_Mananger(WinGlobal_UIService ui)
        {
            UIService = ui;
            WinUser_Account = new WinUser_Account();
            Password_Public = new Password_Public();
          // Password_Private = new Password_Private();
            WinUser_AccountUpdate = new WinUser_AccountUpdate(UIService);
            WinUser_AccountCreate = new WinUser_AccountCreate(UIService);
        }

        public async Task Mananger(CancellationToken token, int ValueUniProgressBar)
        {
            string NameUser = Password_Public.GetUser();
            string PasswordUser = Password_Public.GetPassword();
            //string NameUser = Password_Private.GetUser();
            //string PasswordUser = Password_Private.GetPassword();

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
