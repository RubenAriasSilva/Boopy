using UnityEngine;
using UnityEngine.SceneManagement;

public class perfilControlador : MonoBehaviour
{
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
