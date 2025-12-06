using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BoopyGame
{
    public class AjustesControlador : MonoBehaviour
    {
        public GameObject idiomas;
        public Slider volumeSlider;

        public Toggle toggleEspanol; 
        public Toggle toggleIngles;

        void Awake()
        {
            float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            
            if (volumeSlider != null)
            {
                volumeSlider.value = savedVolume;
            }
        }

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

        public void AcercaDe ()
        {
            Debug.Log("Acerca de");
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

        public void AlMoverScroll(float valor)
        {
            Debug.Log("Valor actual: " + valor);

            // Buscamos la instancia estática de nuestro MusicManager y llamamos a su función SetVolume.
            // El 'valor' que recibe la función es el valor actual del Slider (de 0 a 1).
            if (MusicManager.instance != null)
            {
                MusicManager.instance.SetVolume(valor);
            }
            else
            {
                Debug.LogWarning("MusicManager no encontrado. ");
            }
        }

        public void Regresar ()
        {
            SceneManager.LoadScene("menuPrincipal");
        }

    }
}