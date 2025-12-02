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
        public TextMeshProUGUI gatitosJugador1Contrincante;
        public TextMeshProUGUI gatosJugador1Contrincante;
        public TextMeshProUGUI gatitosJugador2;
        public TextMeshProUGUI gatosJugador2;
        public TextMeshProUGUI gatitosJugador2Contrincante;
        public TextMeshProUGUI gatosJugador2Contrincante;

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

        public void ActualizarContenedor (int gatitos1, int gato1, int gatitos2, int gato2)
        {
            gatitosJugador1.text = gatitos1.ToString();
            gatosJugador1.text = gato1.ToString();
            gatitosJugador1Contrincante.text = gatitos2.ToString();
            gatosJugador1Contrincante.text = gato2.ToString();
            gatitosJugador2.text = gatitos2.ToString();
            gatosJugador2.text = gato2.ToString();
            gatitosJugador2Contrincante.text = gatitos1.ToString();
            gatosJugador2Contrincante.text = gato1.ToString();
        }
    }
}