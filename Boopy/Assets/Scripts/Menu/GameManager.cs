using UnityEngine;

namespace BoopyGame
{
    public class GameManager : MonoBehaviour
    {
        // Singleton para acceder desde cualquier lado
        public static GameManager Instance;

        // Aqui se guarda el modo de juego para ejecutar la plantilla
        public ModoDeJuego ModoSeleccionado; 
        public ConfiguracionPartida confPartidaActual;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Esto hace que el objeto no se destruya al cambiar de escena, para no perder el dato de modo
            }
            else
            {
                Destroy(gameObject);
            }
        }        

        public void CargarMenuPrincipal()
        {
            // Opcional: Resetear variables globales si es necesario
            ModoSeleccionado = ModoDeJuego.Ninguno;
            
            // Cargar la escena 0 (o como se llame tu menú)
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

    }
}