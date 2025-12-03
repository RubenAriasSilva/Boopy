using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace BoopyGame
{
    public class ContenedorVista : MonoBehaviour
    {
        // Asigna esto en el Inspector
        public PartidaControlador partidaControlador;
        public Jugador jugador1;
        public Jugador jugador2;

        [Header("Cantidades de fichas")]
        public TextMeshProUGUI gatitosJugador1;
        public TextMeshProUGUI gatosJugador1;
        public TextMeshProUGUI gatitosJugador1Contrincante;
        public TextMeshProUGUI gatosJugador1Contrincante;
        public TextMeshProUGUI gatitosJugador2;
        public TextMeshProUGUI gatosJugador2;
        public TextMeshProUGUI gatitosJugador2Contrincante;
        public TextMeshProUGUI gatosJugador2Contrincante;

        [Header("Nombres")]
        public TextMeshProUGUI textoJugador1Actual;
        public TextMeshProUGUI textoJugador2Actual;
        public TextMeshProUGUI textoJugador2;
        public TextMeshProUGUI textoJugador1;

        [Header("Toggles de Selección")]
        public Toggle toggleGatitoChicoP1;
        public Toggle toggleGatoteP1;
        public Toggle toggleGatitoChicoP2;
        public Toggle toggleGatoteP2;

        public void Inicializar(PartidaControlador controlador, Jugador j1, Jugador j2)
        {
            partidaControlador = controlador;
            jugador1 = j1;
            jugador2 = j2;
        }

        public void Update()
        {
            if (partidaControlador != null && toggleGatitoChicoP1.isOn) // (jugador 1, tipo 1)
            {
                partidaControlador.ClickEnContenedor(1, 1);
            } 
            if (partidaControlador != null && toggleGatoteP1.isOn)  // (jugador 1, tipo 2)
            {
                partidaControlador.ClickEnContenedor(1, 2);
            } 
            if (partidaControlador != null && toggleGatitoChicoP2.isOn)  // (jugador 2, tipo 1)
            {
                partidaControlador.ClickEnContenedor(2, 1);
            } 
            if (partidaControlador != null && toggleGatoteP2.isOn) 
            {
                partidaControlador.ClickEnContenedor(2, 2);
            }
        }

        public void ResetToggles()
        {
            toggleGatitoChicoP1.isOn = false;
            toggleGatoteP1.isOn = false;
            toggleGatitoChicoP2.isOn = false;
            toggleGatoteP2.isOn = false;
        }

        public void actualizarcontenedores()
        {
            int gatitosJ1 = jugador1.Contenedor.CantGatosPequenos;
            int gatitosJ2 = jugador2.Contenedor.CantGatosPequenos;
            int gatotesJ1 = jugador1.Contenedor.CantGatosGrandes;
            int gatotesJ2 = jugador2.Contenedor.CantGatosGrandes;
            string nombreJ1 = jugador1.Nombre;
            string nombreJ2 = jugador2.Nombre;

            //Poner los nombres de los jugadores
            textoJugador1Actual.text = nombreJ1;
            textoJugador1.text = nombreJ1;
            textoJugador2Actual.text = nombreJ2;
            textoJugador2.text = nombreJ2;

            //Para cuando el contenedor principal sea el del jugador 1
            gatitosJugador1.text = gatitosJ1.ToString();
            gatosJugador1.text = gatotesJ1.ToString();
            gatitosJugador1Contrincante.text = gatitosJ2.ToString();
            gatosJugador1Contrincante.text = gatotesJ2.ToString();

            //Para cuando el contenedor principal sea el del jugador 2
            gatitosJugador2.text = gatitosJ2.ToString();
            gatosJugador2.text = gatotesJ2.ToString();
            gatitosJugador2Contrincante.text = gatitosJ1.ToString();
            gatosJugador2Contrincante.text = gatotesJ1.ToString();
        }
    }
}