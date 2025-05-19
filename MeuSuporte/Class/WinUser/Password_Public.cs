using Microsoft.VisualBasic.ApplicationServices;

namespace MeuSuporte
{
    internal class Password_Public
    {
        private string user = "Suporte";
        private string password = "r46W6h8#";

        public string User // propriedade pública
        {
            get { return user; } // retorna o valor            
        }

        public string Password // propriedade pública
        {
            get { return password; } // retorna o valor            
        }
    }
}