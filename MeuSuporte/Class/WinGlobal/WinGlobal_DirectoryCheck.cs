using System.IO;

namespace MeuSuporte
{
    internal class WinGlobal_DirectoryCheck
    {
        public bool Check(string Path)
        {
            return Directory.Exists(Path);
        }
    }
}
