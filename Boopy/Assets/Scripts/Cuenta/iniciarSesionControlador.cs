using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace BoopyGame
{
    public class iniciarSesionControlador : MonoBehaviour
    {
        public GameObject idiomas;

        public TMP_InputField correo_input;
        public TMP_InputField password_input;
        private string correo;
        private string password;

        public Toggle toggleEspanol; 
        public Toggle toggleIngles;

        void Start()
        {
            int idiomaGuardado = PlayerPrefs.GetInt("IdiomaSeleccionado", 0);
            
            if (idiomaGuardado == 0)
            {
                toggleEspanol.isOn = true;
            }
            else
            {
                toggleIngles.isOn = true;
            }
        }

        public void AbrirIdiomas ()
        {
            idiomas.SetActive(true);
        }

        public void CerrarIdiomas ()
        {
            idiomas.SetActive(false);
        }

        public void SelecionarIdioma(bool idioma) // 0 = español, 1 = ingles
        {
            if (!idioma)
            {
                PlayerPrefs.SetInt("IdiomaSeleccionado", 0);
            }
            else
            {
                PlayerPrefs.SetInt("IdiomaSeleccionado", 1);
            }
            PlayerPrefs.Save(); 
        }

        public void Registrase ()
        {
            SceneManager.LoadScene("registrarse");        
        }


        public void JugarSinConexion()
        {
            GameManager_DB.Instance.UsuarioPredeterminado();
            SceneManager.LoadScene("menuPrincipal");
        }

        public async void IniciarSesion ()
        {
            correo = correo_input.text;
            password = password_input.text;

            bool exito = await GameManager_DB.Instance.ValidarLogin(correo, password);

            if (exito) SceneManager.LoadScene("menuPrincipal");        
        }
    }
}