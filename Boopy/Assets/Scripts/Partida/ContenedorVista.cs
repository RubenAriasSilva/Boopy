using UnityEngine;


namespace BoopyGame
{
    public class ContenedorVista : MonoBehaviour
    {
        public int idJugador; // 1 = jugador1, 2 = jugador2
        public PartidaControlador partidaControlador;

        public void OnClickGatitoChico()
        {
            partidaControlador.SeleccionarGato(idJugador, 1); // 1 = chico
        }

        public void OnClickGatote()
        {
            partidaControlador.SeleccionarGato(idJugador, 2); // 2 = grande
        }
    }
}
