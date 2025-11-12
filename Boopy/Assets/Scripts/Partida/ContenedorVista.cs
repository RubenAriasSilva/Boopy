using UnityEngine;


namespace BoopyGame
{
    public class ContenedorVista : MonoBehaviour
    {
        public PartidaControlador partidaControlador;

        public void OnClickGatitoChico(int idJugador)
        {
            partidaControlador.SeleccionarGato(idJugador, 1); // 1 = chico
        }

        public void OnClickGatote(int idJugador)
        {
            partidaControlador.SeleccionarGato(idJugador, 2); // 2 = grande
        }
    }
}
