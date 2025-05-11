using System;
using System.DirectoryServices;
using System.Reflection;
using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinUser_State
    {
        public async Task State(DirectoryEntry _User, bool state)
        {
            // true == conta desabilitada
            // false == conta abilitada
            object ent = _User.NativeObject;
            Type type = ent.GetType();
            type.InvokeMember("AccountDisabled", BindingFlags.SetProperty, null, ent, new object[] { state });
            _User.CommitChanges();                   
        }
    }
}
