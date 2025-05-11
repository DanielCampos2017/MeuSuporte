using System.Threading.Tasks;

namespace MeuSuporte
{
    class WinUserUAC_Mananger
    {
        WinUserUAC_State UAC_State;

        public WinUserUAC_Mananger(WinGlobal_UIService ui)
        {
            UAC_State = new WinUserUAC_State(ui);
        }
               
        public async Task Mananger(bool State, int ValueUniProgressBar)
        {
            await UAC_State.Notification(State, ValueUniProgressBar);
        }
    }
}
