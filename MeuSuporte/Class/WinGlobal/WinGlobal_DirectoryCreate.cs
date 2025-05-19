using System.IO;

namespace MeuSuporte
{
    internal class WinGlobal_DirectoryCreate
    {
        private readonly WinGlobal_DirectoryCheck DirectoryCheck;
        private readonly WinGlobal_CreateNameFolde CreateNameFolde;

        public WinGlobal_DirectoryCreate()
        {
            DirectoryCheck = new WinGlobal_DirectoryCheck();
            CreateNameFolde = new WinGlobal_CreateNameFolde();
        }

        public bool Create(string NameFolder)
        {
            // cria um caminho valido
            string Path = CreateNameFolde.Folder(NameFolder);

            try
            {
                // verifica se existe diretorio
                if (!DirectoryCheck.Check(Path))
                {
                    // cria diretorio
                    Directory.CreateDirectory(Path);
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return true; // so vai cair aqui se a pasta ja existir
        }


    }
}
