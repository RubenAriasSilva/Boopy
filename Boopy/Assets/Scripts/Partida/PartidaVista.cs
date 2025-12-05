using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace BoopyGame
{
    public class PartidaVista : MonoBehaviour
    {
        PartidaControlador partidaCtr;
        public GameObject ajustes;
        public GameObject finPartida;
        public TextMeshProUGUI ganador;

        public void Inicializar(PartidaControlador controlador)
        {
            partidaCtr = controlador;
        }

        public void AbrirAjustes()
        {
            ajustes.SetActive(true);
        }

        public void CerrarAjustes()
        {
            ajustes.SetActive(false);
        }

        public void TerminarPartida(string nombre)
        {
            finPartida.SetActive(true);
            ganador.text = nombre;
        }

        public void RegresarMenuPrincipal()
        {
            Debug.Log("Saliendo de la partida...");
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CargarMenuPrincipal();
            }
            else
            {             
                SceneManager.LoadScene("menuPrincipal");
            }
        }
    }
}