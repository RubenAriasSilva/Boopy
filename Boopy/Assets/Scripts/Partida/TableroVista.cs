using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic; 
using System.Linq;
using UnityEngine.UI;

namespace BoopyGame
{    
    public class TableroVista : MonoBehaviour
    {
        [Header("Referencias")]
        public TableroModelo tableroModelo;  // Se asigna desde el PartidaControlador
        public List<cosmeticosModelo> listaCosmeticosGatitos;
        public List<cosmeticosModelo> listaCosmeticosGatos;

        private bool colorJugador1; // 0 = rojo, 1 = azul
        private bool colorJugador2;

        [Header("Imagenes Contenedor")]
        public Image imagenGatitoJugador1;
        public Image imagenGatoJugador1;
        public Image imagenGatitoJugador2;
        public Image imagenGatoJugador2;

        PartidaControlador partidaCtr;

        private Transform[,] posicionesTablero;
        private GameObject[,] instanciasGatos;

        private static readonly System.Random random = new System.Random();

        public void Inicializar(PartidaControlador controlador)
        {
            partidaCtr = controlador;
        }

        void Awake()
        {
            int tamanio = TableroModelo.TAMANIO_TABLERO;
            posicionesTablero = new Transform[tamanio, tamanio];
            instanciasGatos = new GameObject[tamanio, tamanio];
            AsignarColor ();

            for (int fila = 0; fila < tamanio; fila++)
            {
                for (int col = 0; col < tamanio; col++)
                {
                    GameObject posicionGO = new GameObject($"Posicion_{fila}_{col}");
                    posicionGO.transform.SetParent(this.transform);
                    posicionGO.transform.position = Posiciones.GatitoPosiciones[fila, col];
                    posicionesTablero[fila, col] = posicionGO.transform;
                }
            }

            // Llamamos al método para actualizar los iconos de la UI
            ActualizarIconosUI();
        }

        private void AsignarColor () // 0 = rojo, 1 = azul
        {
            bool aleatorio = BooleanoAleatorio();
            colorJugador1 = aleatorio;
            colorJugador2 = !aleatorio;
        }

        public static bool BooleanoAleatorio()
        {
            return random.Next() % 2 == 0;
        }
        
        // MÉTODO AUXILIAR para no repetir el código de búsqueda
        private cosmeticosModelo ObtenerModeloCosmetico(int numeroJugador, bool esGatito)
        {
            string idCosmetico = "";
            if (numeroJugador == 1)
            {
                idCosmetico = esGatito ? SeleccionDeCosmeticos.IdGatitoJugador1 : SeleccionDeCosmeticos.IdGatoJugador1;
            }
            else if (numeroJugador == 2)
            {
                idCosmetico = esGatito ? SeleccionDeCosmeticos.IdGatitoJugador2 : SeleccionDeCosmeticos.IdGatoJugador2;
            }

            if (string.IsNullOrEmpty(idCosmetico))
            {
                Debug.LogError($"ID de cosmético no encontrado para Jugador {numeroJugador} ({(esGatito ? "Gatito" : "Gato")}).");
                return null;
            }

            List<cosmeticosModelo> listaCorrecta = esGatito ? listaCosmeticosGatitos : listaCosmeticosGatos;
            cosmeticosModelo modelo = listaCorrecta.Find(c => c.id == idCosmetico);

            if (modelo == null)
            {
                Debug.LogError($"Modelo de cosmético con ID '{idCosmetico}' no encontrado en la lista de {(esGatito ? "Gatitos" : "Gatos")}.");
            }

            return modelo;
        }

        public void ActualizarIconosUI()
        {
            // --- JUGADOR 1 ---

            // Gatito Jugador 1
            var modeloGatitoJ1 = ObtenerModeloCosmetico(1, true);
            if (modeloGatitoJ1 != null && imagenGatitoJugador1 != null)
            {
                // Elegimos el icono según el color del jugador 1
                imagenGatitoJugador1.sprite = colorJugador1 ? modeloGatitoJ1.iconoAzul : modeloGatitoJ1.iconoRojo;
            }

            // Gato Jugador 1
            var modeloGatoJ1 = ObtenerModeloCosmetico(1, false);
            if (modeloGatoJ1 != null && imagenGatoJugador1 != null)
            {
                // Elegimos el icono según el color del jugador 1
                imagenGatoJugador1.sprite = colorJugador1 ? modeloGatoJ1.iconoAzul : modeloGatoJ1.iconoRojo;
            }

            // --- JUGADOR 2 ---

            // Gatito Jugador 2
            var modeloGatitoJ2 = ObtenerModeloCosmetico(2, true);
            if (modeloGatitoJ2 != null && imagenGatitoJugador2 != null)
            {
                // Elegimos el icono según el color del jugador 2
                imagenGatitoJugador2.sprite = colorJugador2 ? modeloGatitoJ2.iconoAzul : modeloGatitoJ2.iconoRojo;
            }

            // Gato Jugador 2
            var modeloGatoJ2 = ObtenerModeloCosmetico(2, false);
            if (modeloGatoJ2 != null && imagenGatoJugador2 != null)
            {
                // Elegimos el icono según el color del jugador 2
                imagenGatoJugador2.sprite = colorJugador2 ? modeloGatoJ2.iconoAzul : modeloGatoJ2.iconoRojo;
            }
        }

        public void copiarTablero(TableroModelo tableroX)
        {
            tableroModelo = tableroX;
        }

        private GameObject ObtenerPrefabCosmetico(int numeroJugador, bool esGatito, bool esAzul)
        {
            // Reutilizamos el método auxiliar para obtener el modelo
            cosmeticosModelo modelo = ObtenerModeloCosmetico(numeroJugador, esGatito);

            if (modelo == null)
            {
                return null; // El error ya se logueó en el método auxiliar
            }

            // Devolver el prefab correcto según el color del jugador
            return esAzul ? modelo.prefab3DAzul : modelo.prefab3DRojo;
        }

        // ... El resto de tu código (ActualizarTableroVisual, SeleccionarCasilla, Posiciones) se queda igual ...
        public void ActualizarTableroVisual()
        {
            if (tableroModelo == null) return; // Salida temprana si no hay modelo

            int tamanio = tableroModelo.GetTamanioTablero();

            for (int fila = 0; fila < tamanio; fila++)
            {
                for (int col = 0; col < tamanio; col++)
                {
                    int valor = tableroModelo.GetGato(fila, col);

                    // Si ya había un gato, lo eliminamos antes de actualizar
                    if (instanciasGatos[fila, col] != null)
                    {
                        Destroy(instanciasGatos[fila, col]);
                        instanciasGatos[fila, col] = null;
                    }

                    // Si la celda está vacía, continuamos a la siguiente
                    if (valor == 0)
                        continue;

                    // Determinar el prefab y la rotación según el valor
                    GameObject prefab = null;
                    Quaternion rotacionActual = Quaternion.identity; // Rotación por defecto

                    switch (valor)
                    {
                        case -1: // Gatito Jugador 1
                            prefab = ObtenerPrefabCosmetico(1, true, colorJugador1);
                            rotacionActual = Posiciones.GatitoRotacion;
                            break;
                        case -2: // Gato Jugador 1
                            prefab = ObtenerPrefabCosmetico(1, false, colorJugador1);
                            rotacionActual = Posiciones.GatoRotacion;
                            break;
                        case 1: // Gatito Jugador 2
                            prefab = ObtenerPrefabCosmetico(2, true, colorJugador2);
                            rotacionActual = Posiciones.GatitoRotacion;
                            break;
                        case 2: // Gato Jugador 2
                            prefab = ObtenerPrefabCosmetico(2, false, colorJugador2);
                            rotacionActual = Posiciones.GatoRotacion;
                            break;
                    }

                    // Instanciar el gato en su posición visual
                    if (prefab != null && posicionesTablero[fila, col] != null)
                    {
                        Vector3 posicionInstancia = posicionesTablero[fila, col].position;
                        
                        if (valor == -2 || valor == 2)
                        {
                            posicionInstancia = Posiciones.GatoPosiciones[fila, col];
                        }

                        GameObject nuevoGato = Instantiate(prefab, posicionInstancia, rotacionActual, this.transform);
                        instanciasGatos[fila, col] = nuevoGato;
                    }
                }
            }
        }

        public void SeleccionarCasilla(string coordenada) 
        {
            int fila = int.Parse(coordenada[0].ToString());
            int columna = int.Parse(coordenada[1].ToString());

            partidaCtr.ClickEnCasilla(fila, columna);            
        }
    }
    
    public static class Posiciones
    {
        // --- ROTACIONES ---
        public static Quaternion GatitoRotacion = Quaternion.Euler(-90f, 90f, 26.797f);
        public static Quaternion GatoRotacion = Quaternion.Euler(-90f, 90f, -86.057f);

        // --- POSICIONES DE LOS GATITOS (Y = 1.4f) ---
        // Se organiza en un array 2D para un fácil acceso por [fila, columna]
        public static readonly Vector3[,] GatitoPosiciones = new Vector3[,]
        {
            { new Vector3(-4.32f, 1.4f, 14.28f), new Vector3(-2.30f, 1.4f, 14.16f), new Vector3(-0.37f, 1.4f, 14f), new Vector3(1.62f, 1.4f, 13.89f), new Vector3(3.54f, 1.4f, 13.78f), new Vector3(5.45f, 1.4f, 13.62f) },
            { new Vector3(-4.37f, 1.4f, 12.34f), new Vector3(-2.41f, 1.4f, 12.25f), new Vector3(-0.52f, 1.4f, 12.1f), new Vector3(1.42f, 1.4f, 11.91f), new Vector3(3.38f, 1.4f, 11.76f), new Vector3(5.27f, 1.4f, 11.63f) },
            { new Vector3(-4.6f, 1.4f, 10.27f), new Vector3(-2.62f, 1.4f, 10.23f), new Vector3(-0.69f, 1.4f, 10.06f), new Vector3(1.28f, 1.4f, 9.94f), new Vector3(3.24f, 1.4f, 9.81f), new Vector3(5.18f, 1.4f, 9.68f) },
            { new Vector3(-4.74f, 1.4f, 8.29f), new Vector3(-2.71f, 1.4f, 8.26f), new Vector3(-0.87f, 1.4f, 8.07f), new Vector3(1.15f, 1.4f, 7.95f), new Vector3(3.08f, 1.4f, 7.86f), new Vector3(5.04f, 1.4f, 7.74f) },
            { new Vector3(-4.8f, 1.4f, 6.39f), new Vector3(-2.92f, 1.4f, 6.26f), new Vector3(-0.93f, 1.4f, 6.09f), new Vector3(1.04f, 1.4f, 5.96f), new Vector3(2.98f, 1.4f, 5.79f), new Vector3(4.85f, 1.4f, 5.64f) },
            { new Vector3(-4.98f, 1.4f, 4.41f), new Vector3(-3f, 1.4f, 4.33f), new Vector3(-1.06f, 1.4f, 4.15f), new Vector3(0.92f, 1.4f, 4.010f), new Vector3(2.83f, 1.4f, 3.84f), new Vector3(4.73f, 1.4f, 3.84f) }
        };

        // --- POSICIONES DE LOS GATOS (Y = 2.2f) ---
        public static readonly Vector3[,] GatoPosiciones = new Vector3[,]
        {
            { new Vector3(-4.39f, 2.2f, 14.11f), new Vector3(-2.49f, 2.2f, 14.03f), new Vector3(-0.51f, 2.2f, 13.86f), new Vector3(1.45f, 2.2f, 13.68f), new Vector3(3.42f, 2.2f, 13.63f), new Vector3(5.3f, 2.2f, 13.49f) },
            { new Vector3(-4.57f, 2.2f, 12.2f), new Vector3(-2.63f, 2.2f, 12.1f), new Vector3(-0.66f, 2.2f, 11.98f), new Vector3(1.3f, 2.2f, 11.84f), new Vector3(3.23f, 2.2f, 11.67f), new Vector3(5.16f, 2.2f, 11.5f) },
            { new Vector3(-4.7f, 2.2f, 10.25f), new Vector3(-2.76f, 2.2f, 10.02f), new Vector3(-0.79f, 2.2f, 9.93f), new Vector3(1.14f, 2.2f, 9.85f), new Vector3(3.16f, 2.2f, 9.7f), new Vector3(5.03f, 2.2f, 9.53f) },
            { new Vector3(-4.8f, 2.2f, 8.12f), new Vector3(-2.92f, 2.2f, 7.99f), new Vector3(-0.94f, 2.2f, 7.87f), new Vector3(1.07f, 2.2f, 7.77f), new Vector3(3f, 2.2f, 7.68f), new Vector3(4.89f, 2.2f, 7.58f) },
            { new Vector3(-4.88f, 2.2f, 6.2f), new Vector3(-2.98f, 2.2f, 6.07f), new Vector3(-1.04f, 2.2f, 5.88f), new Vector3(0.89f, 2.2f, 5.74f), new Vector3(2.86f, 2.2f, 5.56f), new Vector3(4.7f, 2.2f, 5.43f) },
            { new Vector3(-5.1f, 2.2f, 4.23f), new Vector3(-3.13f, 2.2f, 4.08f), new Vector3(-1.16f, 2.2f, 3.96f), new Vector3(0.76f, 2.2f, 3.78f), new Vector3(2.73f, 2.2f, 3.66f), new Vector3(4.6f, 2.2f, 3.57f) }
        };
    }
}