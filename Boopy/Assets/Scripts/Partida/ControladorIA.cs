using System.Collections.Generic;
using UnityEngine;

namespace BoopyGame
{
    public class ControladorIA
    {
        private PartidaControlador controlador;
        private TableroModelo tablero;
        private PartidaVsIA estrategiaPartida;

        // Valores de las fichas para la IA (Jugador 2)
        private readonly int[] tiposDeFichaIA = { 1, 2 }; // 1: Gatito, 2: Gatote

        public ControladorIA(PartidaControlador controlador, TableroModelo tablero, PartidaVsIA estrategia)
        {
            this.controlador = controlador;
            this.tablero = tablero;
            this.estrategiaPartida = estrategia;
        }

        public void RealizarMovimientoAleatorio()
        {
            Jugador ia = controlador.GetJugador2();
            if (ia == null) return;

            // 1. Obtener una lista aleatoria de tipos de ficha para intentar
            List<int> tiposDeFichaAleatorios = new List<int>(tiposDeFichaIA);
            Shuffle(tiposDeFichaAleatorios);

            // 2. Intentar seleccionar y colocar una ficha
            foreach (int tipoFicha in tiposDeFichaAleatorios)
            {
                // Intenta seleccionar la ficha. Si el jugador no tiene, HaSeleccionadoGato será false.
                ia.SeleccionarGato(tipoFicha);

                // 3. Comprobar si la selección fue exitosa
                if (ia.HaSeleccionadoGato)
                {
                    // Si fue exitosa, procedemos a encontrar una casilla y colocar la ficha
                    List<Vector2Int> celdasVacias = ObtenerCeldasVacias();

                    if (celdasVacias.Count > 0)
                    {
                        Vector2Int celdaElegida = celdasVacias[Random.Range(0, celdasVacias.Count)];
                        Debug.Log($"IA elige ficha tipo {tipoFicha} y la coloca en ({celdaElegida.x}, {celdaElegida.y})");
                        
                        // Ejecutar el movimiento
                        controlador.StartCoroutine(estrategiaPartida.RealizarMovimientoLocal(celdaElegida.x, celdaElegida.y));
                        return; // Salimos del método, el movimiento se está realizando
                    }
                    else
                    {
                        // Esto no debería pasar en un juego normal, pero por si acaso
                        Debug.LogWarning("La IA seleccionó una ficha pero no hay celdas vacías.");
                        ia.DeseleccionarGato(); // Deseleccionamos para no dejarla en un estado extraño
                        return;
                    }
                }
            }

            // 4. Si el bucle termina, la IA no tiene fichas disponibles
            Debug.Log("La IA no tiene más fichas para mover. Fin de su turno.");
            // Aquí podrías añadir lógica para que el jugador humano gane automáticamente,
            // o simplemente esperar a que el jugador humano termine su juego.
            // Por ahora, simplemente no hace nada.
        }

        // Método auxiliar para barajar una lista (Fisher-Yates shuffle)
        private void Shuffle<T>(List<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private List<Vector2Int> ObtenerCeldasVacias()
        {
            List<Vector2Int> celdasVacias = new List<Vector2Int>();
            int tamanio = tablero.GetTamanioTablero();

            for (int fila = 0; fila < tamanio; fila++)
            {
                for (int col = 0; col < tamanio; col++)
                {
                    if (tablero.GetGato(fila, col) == 0)
                    {
                        celdasVacias.Add(new Vector2Int(fila, col));
                    }
                }
            }
            return celdasVacias;
        }
    }
}