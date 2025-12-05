
namespace BoopyGame
{
    public interface IEstrategiaPartida
    {   
        void Iniciar(PartidaControlador controlador, TableroModelo tablero, MotorDeReglas motor, PartidaVista partidaVista);
        void ManejarSeleccionFicha(int tipoFicha);
        void ManejarMovimiento(int fila, int col);
        void Actualizar();
        void SiguientePaso(); //Metodo solo para la estategia tutorial
    }
}