using UnityEngine;
using TMPro;

public class IdiomaManager : MonoBehaviour
{
    public string[] textoIngles;
    public string[] textoEspanol;
    public TextMeshProUGUI[] textosUI;

    void Awake()
    {
        AplicarIdioma();
    }

    public void AplicarIdioma()
    {
        // por defecto es 0 (Español).
        int idioma = PlayerPrefs.GetInt("IdiomaSeleccionado", 0);

        if (idioma == 0)
        {
            for (int i = 0; i < textosUI.Length; i++)
            {
                if (textosUI[i] != null && i < textoEspanol.Length)
                {
                    textosUI[i].text = textoEspanol[i];
                }
            }
        }
        else 
        {
            for (int i = 0; i < textosUI.Length; i++)
            {
                if (textosUI[i] != null && i < textoIngles.Length)
                {
                    textosUI[i].text = textoIngles[i];
                }
            }
        }
    }
}