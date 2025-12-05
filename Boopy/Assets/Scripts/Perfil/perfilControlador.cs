using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace BoopyGame
{
    public class perfilControlador : MonoBehaviour
    {
        public string nombre;
        public TextMeshProUGUI nombrePerfil;

        void Start()
        {
            nombre = GameManager_DB.Instance.usuarioActual.Nombre;
            nombrePerfil.text = nombre;
        }


        public void CambiarFoto()
        {
            Debug.Log("Cambiar foto");
        }

        public void CambiarNombre()
        {
            Debug.Log("Cambiar nombre");
        }

        public void Regresar ()
        {
            SceneManager.LoadScene("menuPrincipal");
        }
    }
}