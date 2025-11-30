using UnityEngine;
using UnityEngine.UI;

public class iniciarSesionControlador : MonoBehaviour
{
    public GameObject idiomas;

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
        Debug.Log("Registrase");
    }

    public void IniciarSesion ()
    {
        Debug.Log("Iniciar sesion");
    }
}
