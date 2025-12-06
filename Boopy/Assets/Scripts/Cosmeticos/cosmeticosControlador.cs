using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class cosmeticosControlador : MonoBehaviour
{
    [Header("Configuración de Listas")]
    public List<cosmeticosModelo> cosmeticosGatitos;
    public List<cosmeticosModelo> cosmeticosGatos;
    public GameObject prefabElementoUI;

    [Header("Referencias a la UI")]
    public Transform panelContenido;

    // --- ESTADO DEL JUGADOR Y DEL CONTROLADOR ---

    // Lista de IDs desbloqueados por tipo
    private List<string> idsCosmeticosGatitosDesbloqueados;
    private List<string> idsCosmeticosGatosDesbloqueados;

    // El ID del cosmético equipado por tipo
    private string idGatitoEquipado = "";
    private string idGatoEquipado = "";

    // Variable para saber qué tipo de cosmético se está mostrando
    private bool mostrarGatitos = true;

    void Start()
    {
        // Simulamos que el jugador tiene todos los cosméticos de ambos tipos desbloqueados.
        idsCosmeticosGatitosDesbloqueados = cosmeticosGatitos.Select(c => c.id).ToList();
        idsCosmeticosGatosDesbloqueados = cosmeticosGatos.Select(c => c.id).ToList();

        // Cargamos las selecciones guardadas o asignamos una por defecto
        CargarOSeleccionarPorDefecto();

        // Llamamos a la función para que muestre la lista por defecto (Gatitos)
        PoblarUI(mostrarGatitos);
    }

    void CargarOSeleccionarPorDefecto()
    {
        // --- Gatitos ---
        idGatitoEquipado = PlayerPrefs.GetString("idGatitoEquipado", "");
        if (string.IsNullOrEmpty(idGatitoEquipado) && idsCosmeticosGatitosDesbloqueados.Count > 0)
        {
            idGatitoEquipado = idsCosmeticosGatitosDesbloqueados[0];
            PlayerPrefs.SetString("idGatitoEquipado", idGatitoEquipado); // Guardamos el por defecto para la próxima vez
        }

        // buscar en la tabla gatitos el id
        // actualizar id en usuario
        // update usuario

        // --- Gatos ---
        idGatoEquipado = PlayerPrefs.GetString("idGatoEquipado", "");
        if (string.IsNullOrEmpty(idGatoEquipado) && idsCosmeticosGatosDesbloqueados.Count > 0)
        {
            idGatoEquipado = idsCosmeticosGatosDesbloqueados[0];
            PlayerPrefs.SetString("idGatoEquipado", idGatoEquipado); // Guardamos el por defecto para la próxima vez
        }
    }

    // La función ahora recibe un booleano para saber qué lista mostrar
    void PoblarUI(bool esGatito)
    {
        // Limpiamos el panel por si acaso ya tenía algo
        foreach (Transform hijo in panelContenido)
        {
            Destroy(hijo.gameObject);
        }

        List<cosmeticosModelo> listaActual = esGatito ? cosmeticosGatitos : cosmeticosGatos;
        List<string> idsDesbloqueadosActuales = esGatito ? idsCosmeticosGatitosDesbloqueados : idsCosmeticosGatosDesbloqueados;

        // Poblamos la UI con la lista seleccionada
        foreach (var id in idsDesbloqueadosActuales)
        {
            cosmeticosModelo cosmetico = listaActual.Find(c => c.id == id);
            if (cosmetico != null)
            {
                GameObject elementoUI = Instantiate(prefabElementoUI, panelContenido);
                // Le pasamos la categoría para que sepa qué ID de equipado comprobar
                ConfigurarElementoUI(elementoUI, cosmetico, esGatito);
            }
        }
    }

    // La función ahora recibe la categoría para configurar el estado correctamente
    void ConfigurarElementoUI(GameObject elementoUI, cosmeticosModelo cosmetico, bool esGatito)
    {
        // Obtenemos el índice del idioma actual
        int indiceIdioma = PlayerPrefs.GetInt("IdiomaSeleccionado", 0);
        string nombreAMostrar = (cosmetico.nombresPorIdioma != null && indiceIdioma < cosmetico.nombresPorIdioma.Length)
            ? cosmetico.nombresPorIdioma[indiceIdioma]
            : "Nombre no encontrado";

        // Aplicamos el texto y el icono a la UI
        elementoUI.transform.Find("Nombre").GetComponent<TextMeshProUGUI>().text = nombreAMostrar;
        
        Transform iconoTransform = elementoUI.transform.Find("Icono");
        if (iconoTransform != null)
        {
            Image iconoImage = iconoTransform.GetComponent<Image>();
            if (iconoImage != null)
            {
                iconoImage.sprite = cosmetico.iconoUI;
            }
        }

        // --- LÓGICA DE RESALTADO Y BOTÓN ---
        string idEquipadoActual = esGatito ? idGatitoEquipado : idGatoEquipado;
        bool estaEquipado = (cosmetico.id == idEquipadoActual);

        // Activamos o desactivamos el marco de selección
        Image marcoSeleccion = elementoUI.transform.Find("MarcoSeleccion").GetComponent<Image>();
        if (marcoSeleccion != null)
        {
            marcoSeleccion.enabled = estaEquipado;
        }

        // El botón solo es interactivo si el cosmético no está equipado
        Button boton = elementoUI.GetComponentInChildren<Button>();
        boton.interactable = !estaEquipado;

        boton.onClick.AddListener(() => OnClickBotonEquipar(cosmetico, esGatito));
    }

    // El método ahora recibe la categoría para saber qué lista actualizar
    public void SelecionarTipoFicha(bool esGato)
    {
        mostrarGatitos = !esGato;
        PoblarUI(mostrarGatitos);
    }

    // El método ahora recibe la categoría para guardar el ID correcto
    void OnClickBotonEquipar(cosmeticosModelo cosmeticoClicado, bool esGatito)
    {
        int indiceIdioma = PlayerPrefs.GetInt("IdiomaSeleccionado", 0);
        string nombreAMostrar = (cosmeticoClicado.nombresPorIdioma != null && indiceIdioma < cosmeticoClicado.nombresPorIdioma.Length)
            ? cosmeticoClicado.nombresPorIdioma[indiceIdioma]
            : "Nombre no encontrado";

        if (esGatito)
        {
            idGatitoEquipado = cosmeticoClicado.id;
            PlayerPrefs.SetString("idGatitoEquipado", idGatitoEquipado);
            Debug.Log($"Gatito equipado y guardado: {nombreAMostrar}");
        }
        else
        {
            idGatoEquipado = cosmeticoClicado.id;
            PlayerPrefs.SetString("idGatoEquipado", idGatoEquipado);
            Debug.Log($"Gato equipado y guardado: {nombreAMostrar}");
        }
        
        // Refrescamos la UI de la categoría actual para mostrar el cambio
        PoblarUI(esGatito);
    }

    public void Regresar ()
    {
        SceneManager.LoadScene("menuPrincipal");
    }
}