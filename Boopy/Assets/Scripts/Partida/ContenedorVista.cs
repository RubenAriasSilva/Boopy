using UnityEngine;


namespace BoopyGame
{
    public class ContenedorVista : MonoBehaviour
    {
        public PartidaControlador partidaControlador;

        public void OnClickGatitoChico(int idJugador)
        {
            int id = partidaControlador.idJugadorAcutal();
            if(id == idJugador)
            {
                Debug.Log("Gatito seleccionado: P" + idJugador);
                partidaControlador.SeleccionarGato(idJugador, 1); // 1 = chico    
            } else
            {
                Debug.Log("No es turno de este jugador");
                return;
            }
            
            
        }

        public void OnClickGatote(int idJugador)
        {
            int id = partidaControlador.idJugadorAcutal();
            if(id == idJugador)
            {
                Debug.Log("Gatote seleccionado: P" + idJugador);
                partidaControlador.SeleccionarGato(idJugador, 2); // 2 = grande    
            } else
            {
                Debug.Log("No es turno de este jugador");
            }

            
        }
    }
}
