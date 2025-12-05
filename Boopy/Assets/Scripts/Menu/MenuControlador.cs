using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace BoopyGame
{
public class MenuControlador : MonoBehaviour
{
    public List<cosmeticosModelo> cosmeticosGatitos;
    public List<cosmeticosModelo> cosmeticosGatos;

    private List<string> idsCosmeticosGatitosDesbloqueados;
    private List<string> idsCosmeticosGatosDesbloqueados;

    void Start()
    {
        // 1. Cargar los cosméticos del Jugador 1 desde PlayerPrefs (igual que antes)
        string idGatitoJ1 = PlayerPrefs.GetString("idGatitoEquipado", "");
        string idGatoJ1 = PlayerPrefs.GetString("idGatoEquipado", "");

        // 2. Asignarlos a la clase estática
        SeleccionDeCosmeticos.IdGatitoJugador1 = idGatitoJ1;
        SeleccionDeCosmeticos.IdGatoJugador1 = idGatoJ1;

        SeleccionDeCosmeticos.IdGatitoJugador2 = idGatitoJ1;
        SeleccionDeCosmeticos.IdGatoJugador2 = idGatoJ1;
    }

    public void JugarPartidaLocal()
    {
        //Debug.Log("Local");
        //GameManager.Instance.ModoSeleccionado = ModoDeJuego.Local;        
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("prePartidaLocal");
    }

    //public void JugarPartidaMultijugador()
    //{
    //    SceneManager.LoadScene("prePartidaEnLinea");
    //}

    public void JugarPartidaVsIA()
    {
        Debug.Log("vs IA");
    }

    public void JugarPartidaTutorial()
    {
        Debug.Log("Tutorial");
        GameManager.Instance.ModoSeleccionado = ModoDeJuego.Tutorial;
        SceneManager.LoadScene("partida");
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
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego . . .");
        Application.Quit();
    }
}
}