using UnityEngine;
using UnityEngine.SceneManagement;

public class clasificacionesControlador : MonoBehaviour
{
    public void SelecionarTipoClasificacion(bool tipo) // 0 Mundial, 1 Amigos
    {
        if (tipo)
        {
            Debug.Log("Amigos");
        } else
        {
            Debug.Log("Mundial");
        }
    }

    public void Regresar ()
    {
        SceneManager.LoadScene("menuPrincipal");
    }
}
