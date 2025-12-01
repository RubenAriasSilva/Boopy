using UnityEngine;
using TMPro;


namespace BoopyGame
{
    public class ContenedorVista : MonoBehaviour
    {
        // Asigna esto en el Inspector
        public PartidaControlador partidaControlador;
        public TextMeshProUGUI gatitosJugador1;
        public TextMeshProUGUI gatosJugador1;
        public TextMeshProUGUI gatitosJugador2;
        public TextMeshProUGUI gatosJugador2;

        public void Inicializar(PartidaControlador controlador)
        {
            partidaControlador = controlador;
        }
                
        public void OnClickGatitoChicoP1()
        {            
            if(partidaControlador != null) partidaControlador.ClickEnContenedor(1, 1); // (jugador 1, tipo 1)
        }
        
        public void OnClickGatoteP1()
        {
            if(partidaControlador != null) partidaControlador.ClickEnContenedor(1, 2); // (jugador 1, tipo 2)
        }
        
        public void OnClickGatitoChicoP2()
        {
            if(partidaControlador != null) partidaControlador.ClickEnContenedor(2, 1); // (jugador 2, tipo 1)
        }

        public void OnClickGatoteP2 ()
        {
            if(partidaControlador != null) partidaControlador.ClickEnContenedor(2, 2); // (jugador 2, tipo 2)
        }

        public void ActualizarContenedor (int gatitos1, int gato1, int gatitos2, int gatos2)
        {
            gatitosJugador1.text = gato1.ToString();
            gatosJugador1.text = gato1.ToString();
            gatitosJugador2.text = gatitos2.ToString();
            gatosJugador2.text = gatos2.ToString();
        }
    }
}