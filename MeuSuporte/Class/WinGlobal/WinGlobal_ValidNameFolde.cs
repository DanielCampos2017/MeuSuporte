using System;
using System.IO;
using System.Linq;

namespace MeuSuporte
{
    internal class WinGlobal_ValidNameFolde
    {
        // Verifica se o nome da pasta é valido para Windows 
        public bool Valid(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name)) return false;

            // Lista de caracteres inválidos no Windows
            char[] InvalidCharacters = Path.GetInvalidFileNameChars();

            // retorna todos os caracteres especiais inválido < > : " / \ | ? * 
            if (Name.IndexOfAny(InvalidCharacters) != -1) return false;

            // Lista de nomes reservados no Windows
            string[] NameRestricted = {
            "CON", "PRN", "AUX", "NUL",
            "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
             };

            // Verifica se o nome está na lista de reservados (sem diferenciar maiúsculas de minúsculas)
            if (NameRestricted.Contains(Name.ToUpper())) return false;
            if (NameRestricted.Contains(Name.ToUpperInvariant())) return false;

            // Nome não pode terminar com espaço ou ponto
            if (Name.EndsWith(" ") || Name.EndsWith(".")) return false;

            return true;
        }
    }
}
