namespace MeuSuporte
{
    internal class WinGlobal_DirectoryMananger
    {
        private WinGlobal_DirectoryCheck DirectoryCheck;
        private WinGlobal_DirectoryCreate DirectoryCreate;
        private WinGlobal_CreateNameFolde CreateNameFolde;

        public WinGlobal_DirectoryMananger()
        {
            DirectoryCreate = new WinGlobal_DirectoryCreate();
            CreateNameFolde = new WinGlobal_CreateNameFolde();
            DirectoryCheck = new WinGlobal_DirectoryCheck();
        }

        // verifica diretorio
        public bool Check(string Path)
        {
            return DirectoryCheck.Check(Path);
        }

        // cria diretorio
        public bool Create(string NameFolder)
        {
            return DirectoryCreate.Create(NameFolder); 
        }

        public string GetDirectory(string NameFolder)
        {
            return CreateNameFolde.Folder(NameFolder); ;
        }
    }
}
