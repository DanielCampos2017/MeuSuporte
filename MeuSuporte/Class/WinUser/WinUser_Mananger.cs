using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinUser_Mananger
    {
        private WinUser_Account WinUser_Account;
        private Password_Public Password_Public;
        //private  Password_Private Password_Private;
        private WinUser_AccountUpdate WinUser_AccountUpdate;
        private WinUser_AccountCreate WinUser_AccountCreate;

        public WinUser_Mananger()
        {
            WinUser_Account = new WinUser_Account();
            Password_Public = new Password_Public();
          // Password_Private = new Password_Private();
            WinUser_AccountUpdate = new WinUser_AccountUpdate();
            WinUser_AccountCreate = new WinUser_AccountCreate();
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
