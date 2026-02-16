using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;

namespace BoopyGame
{
    public class perfilControlador : MonoBehaviour
    {        
        public TMP_InputField nombre_input;
        public TextMeshProUGUI nombrePerfil;

        void Start()
        {
            string nombre = getUsuarioNombre();
            CambiarNombre(nombre);
        }


        public void CambiarFoto()
        {
            Debug.Log("Cambiar foto");
        }

        private string getUsuarioNombre()
        {
            return GameManager_DB.Instance.usuarioActual.Nombre;
        }

        private void CambiarNombre(string n)
        {
            nombrePerfil.text = n;
        }

        public async void GuardarCambios()
        {
            string nombre = nombre_input.text;
            CambiarNombre(nombre);            

            bool exito = await GuardarCambiosBD(nombre);
            if(!exito) return;
        }

        private async Task<bool> GuardarCambiosBD(string nombre)
        {
            if(nombre == null) return false;

            bool conexion = GameManager_DB.Instance.hayConexion();

            if (!conexion)
            {
                GameManager_DB.Instance.UsuarioPredeterminado(nombre);
                return true;
            }
            
            GameManager_DB.Instance.usuarioActual.Nombre = nombre;

            bool exito = await GameManager_DB.Instance.CambiarUsuario();

            if (exito)
            {
                Debug.Log("Nombre actualizado en tabla");
                return true;
            } else
            {
                Debug.Log("Nombre no se logro actualizar");
                return false;
            }
        }

        public void Regresar ()
        {
            SceneManager.LoadScene("menuPrincipal");
        }
    }
}