using System.IO;
using System.Reflection;

namespace MeuSuporte
{
    class Class_ExecutionPath
    { 
        public bool getDiscoverNetwork()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string rootPath = Path.GetPathRoot(exePath);

            if (exePath.StartsWith(@"\\"))
            {
                return true; // Executado a partir pasta compartilhamentos de rede (\\servidor\pasta\)             
            }

            DriveInfo drive = new DriveInfo(rootPath);
            if (drive.DriveType == DriveType.Network)
            {
                return true; // Executado a partir de um Drive de rede ex. Z:\
            }

            return false;
        }
    }
}
