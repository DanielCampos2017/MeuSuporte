using System.Threading;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinUser_Mananger
    {
        private readonly WinUser_Account WinUser_Account;
        //private readonly Password_Public Password_Public;
        private readonly   Password_Private Password_Private;
        private readonly WinUser_AccountUpdate WinUser_AccountUpdate;
        private readonly WinUser_AccountCreate WinUser_AccountCreate;

        public WinUser_Mananger()
        {
            WinUser_Account = new WinUser_Account();
            //Password_Public = new Password_Public();
            Password_Private = new Password_Private();
            WinUser_AccountUpdate = new WinUser_AccountUpdate();
            WinUser_AccountCreate = new WinUser_AccountCreate();
        }

        public async Task Mananger()
        {
            //string NameUser = Password_Public.Password;
            //string PasswordUser = Password_Public.User;
            string NameUser = Password_Private.User;
            string PasswordUser = Password_Private.Password;

            // verifica se existe usuario
            if (await WinUser_Account.IsEnabled(NameUser))
            {
                await WinUser_AccountUpdate.Update(WinGlobal_UIService.Instance.ValueUniProgressBar, NameUser, PasswordUser);
                return;
            }                                  
           
            await WinUser_AccountCreate.Create(WinGlobal_UIService.Instance.ValueUniProgressBar, NameUser, PasswordUser);
        }
    }
}
