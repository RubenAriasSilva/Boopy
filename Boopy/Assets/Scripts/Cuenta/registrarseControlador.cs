using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BoopyGame
{
    public class registrarseControlador : MonoBehaviour
    {
        public GameObject idiomas;

        public TMP_InputField nickName_input;
        public TMP_InputField correo_input;
        public TMP_InputField password_input;
        public TMP_InputField password2_input;

        private string nickName;
        private string correo;
        private string password;
        private string password2;

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

        public async void Registrase ()
        {
            nickName = nickName_input.text;
            correo = correo_input.text;
            password = password_input.text;
            password2 = password2_input.text;

            if(password != password2)
            {
              Debug.Log("Contrasenias distintas");
              return;  
            } 

            bool exito = await GameManager_DB.Instance.CrearUsuario(nickName, correo, password);
            
            if (exito)
            {
                Debug.Log("¡Cuenta creada! Cambiando de escena...");
            }
            else
            {
                Debug.Log("Hubo un error al registrarse. Intenta de nuevo.");
            }            
        }

        public void IniciarSesion ()
        {
            Debug.Log("Iniciar sesion");
        }
    }
}