using System.Threading.Tasks;

namespace MeuSuporte
{
    class CheckBuild_Mananger
    {
        private string GitHubRepo = "DanielCampos2017/MeuSuporte";
        private CheckBuild_BuildView BuildView;

        public CheckBuild_Mananger()
        {
            BuildView = new CheckBuild_BuildView();
        }

        public async Task Build()
        {
            await Task.Run(() => BuildView.VersionBuild(GitHubRepo));
        }
    }
}