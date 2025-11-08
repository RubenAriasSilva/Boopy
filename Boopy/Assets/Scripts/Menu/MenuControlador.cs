using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControlador : MonoBehaviour
{
    public void JugarPartidaLocal()
    {
        Debug.Log("Local");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void JugarPartidaMultijugador()
    {
        Debug.Log("Multijugador");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void JugarPartidaVsIA()
    {
        Debug.Log("vs IA");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void JugarPartidaTutorial()
    {
        Debug.Log("Tutorial");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego . . .");
        Application.Quit();
    }
}
