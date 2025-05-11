using System.Threading.Tasks;

namespace MeuSuporte
{
    internal class WinBackupDriver_ValueUnit
    {
        private static float accumulator = 0f;
        public async Task<int> Value(float valor)
        {
            accumulator += valor;

            // Extrai a parte inteira e armazena na ProgressBar
            int parteInteira = (int)accumulator;

            if (parteInteira > 0)
            {
                accumulator -= parteInteira;
                return parteInteira;
            }
            return 0;
        }
    }
}
