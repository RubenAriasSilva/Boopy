using UnityEngine;
using UnityEngine.SceneManagement;

public class partidaEnLineaControlador : MonoBehaviour
{
    public GameObject crarSala;
    public GameObject UnirseSala;

    public void SelecionarTipoSala(bool sala) // 0 crear, 1 unirse
    {
        if (!sala){
            crarSala.SetActive(true);
            UnirseSala.SetActive(false);
        } else{
            UnirseSala.SetActive(true);
            crarSala.SetActive(false);
        }
    }

    public void Regresar ()
    {
        SceneManager.LoadScene("menuPrincipal");
    }
}
