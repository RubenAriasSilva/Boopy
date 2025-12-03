namespace BoopyGame
{
    public class ResultadoLinea
    {
        public enum Tipo { NO_LINEA, LINEA_NORMAL, LINEA_GANADORA }
        public Tipo tipo;
        public int[,] coords;

        public ResultadoLinea()
        {
            tipo = Tipo.NO_LINEA;
            coords = new int[3, 2];
        }
            
        public ResultadoLinea(Tipo t, int[,] c)
        {
            tipo = t;
            coords = c;
        }
    }

    public class CambioBoop
    {
        public int filaOrigen, colOrigen, filaDestino, colDestino;
        public bool fueraDelTablero;
        public int gatoEmpujado;

        public CambioBoop(int fo, int co, int fd, int cd, bool fuera, int gato)
        {
            filaOrigen = fo;
            colOrigen = co;
            filaDestino = fd;
            colDestino = cd;
            fueraDelTablero = fuera;
            gatoEmpujado = gato;
        }
    }
}
