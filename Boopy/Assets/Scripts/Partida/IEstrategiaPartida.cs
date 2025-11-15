
namespace BoopyGame
{
    public interface IEstrategiaPartida
    {   
        void Iniciar(PartidaControlador controlador, TableroModelo tablero, MotorDeReglas motor);
        void ManejarSeleccionFicha(int tipoFicha);
        void ManejarMovimiento(int fila, int col);
        void Actualizar(); 
    }
}