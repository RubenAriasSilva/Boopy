using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Corrutinas

public class InicioControlador : MonoBehaviour
{
    IEnumerator Start()
    {
        // Espera 6 segundos reales
        yield return new WaitForSeconds(4f);

        // Cambia la escena
        SceneManager.LoadScene("menuPrincipal");
    }
}