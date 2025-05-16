using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class CheckBuild_GetGitHub
    {
        private CheckBuild_CheckNetwork CheckNetwork;
        private CheckBuild_GetVersion GetVersion;

        public CheckBuild_GetGitHub()
        {
            CheckNetwork = new CheckBuild_CheckNetwork();
            GetVersion = new CheckBuild_GetVersion();
        }

        public async Task<string> GetLatestVersion(string repository)
        {
            // Verifica a conectividade com a internet
            if (await CheckNetwork.IsInternetAvailable())
            {
                // checa a versão no gitHub
                string Buildgithub = GetVersion.GetLatestVersionFromGitHub(repository).Result;
                return Buildgithub;
            }
            return "0.0.0.0";
        }
    }
}
