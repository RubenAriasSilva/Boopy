using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace BoopyGame
{
    public class prePartidaControlador : MonoBehaviour
    {
        [Header("Configuración de Listas")]
        public List<cosmeticosModelo> cosmeticosGatitos;
        public List<cosmeticosModelo> cosmeticosGatos;
        public GameObject prefabElementoUI;

        [Header("Referencias a la UI")]
        public GameObject panelCosmeticos; // El panel principal del menú
        public Transform panelContenido;    // El objeto "Content" dentro del Scroll View

        // --- ESTADO DE LOS JUGADORES ---
        // Variables para guardar la selección de cada jugador en esta sesión
        private string idGatitoEquipadoJugador1 = "";
        private string idGatoEquipadoJugador1 = "";
        private string idGatitoEquipadoJugador2 = "";
        private string idGatoEquipadoJugador2 = "";

        // --- CONTROLADOR ---
        private bool menuActivo = false;
        private int currentPlayerIndex = 0; // 0 = Jugador 1, 1 = Jugador 2
        private bool mostrarGatitos = true;

        public Toggle toggleGatitos;
        public Toggle toggleGatos;

        void Start()
        {
            // Simulamos que los jugadores tienen todos los cosméticos desbloqueados
            List<string> idsGatitosDesbloqueados = cosmeticosGatitos.Select(c => c.id).ToList();
            List<string> idsGatosDesbloqueados = cosmeticosGatos.Select(c => c.id).ToList();

            // Asignamos una selección por defecto para ambos jugadores
            if (idsGatitosDesbloqueados.Count > 0)
            {
                idGatitoEquipadoJugador1 = idsGatitosDesbloqueados[0];
                idGatitoEquipadoJugador2 = idsGatitosDesbloqueados[0];
            }
            if (idsGatosDesbloqueados.Count > 0)
            {
                idGatoEquipadoJugador1 = idsGatosDesbloqueados[0];
                idGatoEquipadoJugador2 = idsGatosDesbloqueados[0];
            }
        }

        public void Iniciar ()
        {
            SeleccionDeCosmeticos.IdGatitoJugador1 = this.idGatitoEquipadoJugador1;
            SeleccionDeCosmeticos.IdGatoJugador1 = this.idGatoEquipadoJugador1;
            SeleccionDeCosmeticos.IdGatitoJugador2 = this.idGatitoEquipadoJugador2;
            SeleccionDeCosmeticos.IdGatoJugador2 = this.idGatoEquipadoJugador2;

            GameManager.Instance.ModoSeleccionado = ModoDeJuego.Local;        
            SceneManager.LoadScene("partida");
        }

        // --- MÉTODOS PRINCIPALES ---

        // Este método se llama desde los botones "Jugador 1" y "Jugador 2"
        public void cambiarGatos(bool jugador) // false = jugador1, true = jugador2
        {
            if (menuActivo) return; // Evita abrir el menú si ya está abierto

            // Establecemos qué jugador estamos configurando
            currentPlayerIndex = jugador ? 1 : 0;
            Debug.Log($"Abriendo cosméticos para el Jugador {currentPlayerIndex + 1}");

            menuActivo = true;
            panelCosmeticos.SetActive(true);
            
            // Por defecto, mostramos la pestaña de gatitos al abrir
            mostrarGatitos = true;
            PoblarUI();
            toggleGatitos.isOn = true;
        }

        public void CerrarCosmeticos()
        {
            if (!menuActivo) return; // Evita cerrar el menú si ya está cerrado

            menuActivo = false;
            panelCosmeticos.SetActive(false);
        }

        public void Regresar()
        {
            SceneManager.LoadScene("menuPrincipal");
        }

        // --- LÓGICA DE LA UI DE COSMÉTICOS ---

        public void SelecionarTipoFicha(bool esGato)
        {
            mostrarGatitos = !esGato; // Si esGato es true, mostrarGatitos es false.
            PoblarUI();
        }

        void PoblarUI()
        {
            // Limpiamos el panel
            foreach (Transform hijo in panelContenido)
            {
                Destroy(hijo.gameObject);
            }

            // Decidimos qué lista y qué IDs de jugador usar
            List<cosmeticosModelo> listaActual = mostrarGatitos ? cosmeticosGatitos : cosmeticosGatos;
            List<string> idsDesbloqueadosActuales = mostrarGatitos ? cosmeticosGatitos.Select(c => c.id).ToList() : cosmeticosGatos.Select(c => c.id).ToList();

            // Poblamos la UI
            foreach (var id in idsDesbloqueadosActuales)
            {
                cosmeticosModelo cosmetico = listaActual.Find(c => c.id == id);
                if (cosmetico != null)
                {
                    GameObject elementoUI = Instantiate(prefabElementoUI, panelContenido);
                    ConfigurarElementoUI(elementoUI, cosmetico);
                }
            }
        }

        void ConfigurarElementoUI(GameObject elementoUI, cosmeticosModelo cosmetico)
        {
            // Obtenemos el índice del idioma actual
            int indiceIdioma = PlayerPrefs.GetInt("IdiomaSeleccionado", 0);
            string nombreAMostrar = (cosmetico.nombresPorIdioma != null && indiceIdioma < cosmetico.nombresPorIdioma.Length)
                ? cosmetico.nombresPorIdioma[indiceIdioma]
                : "Nombre no encontrado";

            // Aplicamos el texto y el icono
            elementoUI.transform.Find("Nombre").GetComponent<TextMeshProUGUI>().text = nombreAMostrar;
            Transform iconoTransform = elementoUI.transform.Find("Icono");
            if (iconoTransform != null)
            {
                Image iconoImage = iconoTransform.GetComponent<Image>();
                if (iconoImage != null) iconoImage.sprite = cosmetico.iconoUI;
            }

            // Obtenemos el ID del cosmético equipado para el jugador ACTUAL
            string idEquipadoActual = GetEquippedIdForCurrentPlayer(mostrarGatitos);
            bool estaEquipado = (cosmetico.id == idEquipadoActual);

            // Activamos o desactivamos el marco de selección
            Image marcoSeleccion = elementoUI.transform.Find("MarcoSeleccion").GetComponent<Image>();
            if (marcoSeleccion != null) marcoSeleccion.enabled = estaEquipado;

            // El botón solo es interactivo si el cosmético no está equipado
            Button boton = elementoUI.GetComponentInChildren<Button>();
            boton.interactable = !estaEquipado;
            boton.onClick.AddListener(() => OnClickBotonEquipar(cosmetico));
        }

        void OnClickBotonEquipar(cosmeticosModelo cosmeticoClicado)
        {
            int indiceIdioma = PlayerPrefs.GetInt("IdiomaSeleccionado", 0);
            string nombreAMostrar = (cosmeticoClicado.nombresPorIdioma != null && indiceIdioma < cosmeticoClicado.nombresPorIdioma.Length)
                ? cosmeticoClicado.nombresPorIdioma[indiceIdioma]
                : "Nombre no encontrado";

            // Actualizamos la variable correcta según el jugador y el tipo
            if (mostrarGatitos)
            {
                if (currentPlayerIndex == 0) idGatitoEquipadoJugador1 = cosmeticoClicado.id;
                else idGatitoEquipadoJugador2 = cosmeticoClicado.id;
                Debug.Log($"Jugador {currentPlayerIndex + 1} - Gatito equipado: {nombreAMostrar}");
            }
            else
            {
                if (currentPlayerIndex == 0) idGatoEquipadoJugador1 = cosmeticoClicado.id;
                else idGatoEquipadoJugador2 = cosmeticoClicado.id;
                Debug.Log($"Jugador {currentPlayerIndex + 1} - Gato equipado: {nombreAMostrar}");
            }
            
            // Refrescamos la UI para mostrar el cambio
            PoblarUI();
        }

        // Función de ayuda para obtener el ID correcto
        string GetEquippedIdForCurrentPlayer(bool esGatito)
        {
            if (esGatito)
            {
                return (currentPlayerIndex == 0) ? idGatitoEquipadoJugador1 : idGatitoEquipadoJugador2;
            }
            else
            {
                return (currentPlayerIndex == 0) ? idGatoEquipadoJugador1 : idGatoEquipadoJugador2;
            }
        }
    }
}
