namespace SistemaGS.WebAssembly.Extensiones
{
    public class Utilidades
    {
        public static int CalcularDistanciaLevenshtein(string fuente, string destino)
        {
            if (string.IsNullOrEmpty(fuente))
                return string.IsNullOrEmpty(destino) ? 0 : destino.Length;

            if (string.IsNullOrEmpty(destino))
                return fuente.Length;

            int[,] matriz = new int[fuente.Length + 1, destino.Length + 1];

            // Inicializar filas y columnas
            for (int i = 0; i <= fuente.Length; i++)
                matriz[i, 0] = i;

            for (int j = 0; j <= destino.Length; j++)
                matriz[0, j] = j;

            // Rellenar matriz
            for (int i = 1; i <= fuente.Length; i++)
            {
                for (int j = 1; j <= destino.Length; j++)
                {
                    int costo = (fuente[i - 1] == destino[j - 1]) ? 0 : 1;

                    matriz[i, j] = Math.Min(
                        Math.Min(matriz[i - 1, j] + 1,      // eliminación
                                 matriz[i, j - 1] + 1),     // inserción
                        matriz[i - 1, j - 1] + costo        // sustitución
                    );
                }
            }

            return matriz[fuente.Length, destino.Length];
        }
    }
}
