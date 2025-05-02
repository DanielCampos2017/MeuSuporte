using System.IO;
using DocumentFormat.OpenXml.Bibliography;

namespace MeuSuporte
{
    internal class Class_WinDirectory_Path
    {
        public bool CheckPath(string Path)
        {
            return Directory.Exists(Path);
        }

        public bool CreatePath(string Path)
        {
            bool retorno = true;
            try
            {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return retorno;
        }
    }
}
