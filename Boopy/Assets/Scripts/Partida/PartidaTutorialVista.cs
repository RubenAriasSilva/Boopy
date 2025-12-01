using UnityEngine;

namespace BoopyGame
{    
public class PartidaTutorialVista : MonoBehaviour
{
    PartidaControlador partidaCtr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Inicializar(PartidaControlador controlador)
    {
        partidaCtr = controlador;
    }

    public void clickEnContinuarPaso()
    {
        partidaCtr.SiguientePasoTutorial();
    }
}
}