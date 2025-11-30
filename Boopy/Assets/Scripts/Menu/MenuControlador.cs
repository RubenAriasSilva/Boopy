using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace BoopyGame
{
public class MenuControlador : MonoBehaviour
{
    public void JugarPartidaLocal()
    {
<<<<<<< Updated upstream
        Debug.Log("Local");
        GameManager.Instance.ModoSeleccionado = ModoDeJuego.Local;        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
=======
        SceneManager.LoadScene("prePartidaLocal");
>>>>>>> Stashed changes
    }

    public void JugarPartidaMultijugador()
    {
        SceneManager.LoadScene("prePartidaEnLinea");
    }

    public void JugarPartidaVsIA()
    {
        Debug.Log("vs IA");
    }

    public void JugarPartidaTutorial()
    {
        Debug.Log("Tutorial");
<<<<<<< Updated upstream
        GameManager.Instance.ModoSeleccionado = ModoDeJuego.Tutorial;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
=======
    }


    public void AbrirPerfil()
    {
        SceneManager.LoadScene("perfil");
    }

    public void AbrirClasificaciones()
    {
        SceneManager.LoadScene("clasificaciones");
    }

    public void AbrirCosmeticos()
    {
        SceneManager.LoadScene("cosmeticos");
    }

    public void AbrirAjustes()
    {
        SceneManager.LoadScene("ajustes");
>>>>>>> Stashed changes
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego . . .");
        Application.Quit();
    }
}
}