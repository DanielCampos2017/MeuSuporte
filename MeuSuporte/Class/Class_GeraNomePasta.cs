using System;
using System.IO;
using System.Linq;

namespace TaskScheduler
{
    internal class Class_GeraNomePasta
    {
       public string Directory()
        {
            string _Directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + $@"Backup-{DateTime.Now.ToString("dd-MM-yyyy")}");

            string Nome = Environment.MachineName;
            if (!EhNomeDePastaValido(Nome))
            {
                Nome = "Desktop";     //Nome de pasta inválido para os padroes de pasta do Windows
            }

            _Directory += @"\" + Nome + @"\";    //Cria a pasta com o nome do computador

             return _Directory;
        }

        static bool EhNomeDePastaValido(string nomePasta)
        {
            if (string.IsNullOrWhiteSpace(nomePasta)) return false;

            // Lista de caracteres inválidos no Windows
            char[] caracteresInvalidos = Path.GetInvalidFileNameChars();

            // Verifica se há algum caractere inválido
            if (nomePasta.IndexOfAny(caracteresInvalidos) != -1) return false;

            // Lista de nomes reservados no Windows
            string[] nomesReservados = {
            "CON", "PRN", "AUX", "NUL",
            "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
        };

            // Verifica se o nome está na lista de reservados (sem diferenciar maiúsculas de minúsculas)
            if (nomesReservados.Contains(nomePasta.ToUpper())) return false;

            // Nome não pode terminar com espaço ou ponto
            if (nomePasta.EndsWith(" ") || nomePasta.EndsWith(".")) return false;

            return true;
        }

    }
}
