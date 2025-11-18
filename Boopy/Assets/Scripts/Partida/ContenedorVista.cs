using UnityEngine;


namespace BoopyGame
{
    public class ContenedorVista : MonoBehaviour
    {
        // Asigna esto en el Inspector
        public PartidaControlador partidaControlador; 
                
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
    }
}