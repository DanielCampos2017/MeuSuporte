using System.Threading.Tasks;

namespace MeuSuporte
{
    class WinUserUAC_Mananger
    {
        private  WinUserUAC_State UAC_State;
     
        public async Task Mananger(bool State)
        {
            UAC_State = new WinUserUAC_State();
            await UAC_State.Notification(State, WinGlobal_UIService.Instance.ValueUniProgressBar);
        }
    }
}
