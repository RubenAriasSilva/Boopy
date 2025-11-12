using UnityEngine;
using System.Collections.Generic;

namespace BoopyGame
{
    public class PartidaControlador : MonoBehaviour
    {
        private bool juegoTerminado = false;

        private TableroModelo tablero;
        //private TableroVista tableroVista;
        private MotorDeReglas motorDeReglas;
        private Jugador jugador1;
        private Jugador jugador2;
        private Jugador jugadorActual;

        private Jugadores turnoActual;
        private int ganador = 0;        

        // --- Enumeración para los jugadores ---
        private enum Jugadores { JUGADOR1 = 1, JUGADOR2 = 2}

        private enum ValoresGatos { P1_GATITO = -1, P1_GATOTE = -2, P2_GATITO = 1, P2_GATOTE = 2 }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // Inicialización de los objetos
            tablero = new TableroModelo();
            //tableroVista.tableroModelo = tablero;
            //tableroVista = new TableroVista(tablero);
            motorDeReglas = new MotorDeReglas(tablero);

            jugador1 = new Jugador((int)Jugadores.JUGADOR1, -1, -2);
            jugador2 = new Jugador((int)Jugadores.JUGADOR2, 1, 2);
            jugadorActual = jugador1;            

            turnoActual = Jugadores.JUGADOR1;

            Debug.Log("Partida iniciada");
        }

        public bool SeleccionarGato(int idJugador, int gato)
        {
            if ((int)turnoActual == idJugador)
            {
                // Convertimos el valor de la ficha para su respectivo jugador
                gato *= jugadorActual.ValorGatito;
                if (jugadorActual.SeleccionarGato(gato))
                {
                    return true;
                }
            }
            return false;
        }

        public bool realizarMovimiento(int fila, int col)
        {
            //Revisamos si el movimiento es valido
            if (!tablero.SetGato(jugadorActual.GatoSeleccionado, fila, col))
            {
                return false;
            }

            //Restamos gato del contenedor
            jugadorActual.QuitarGatoDelContenedor();

            // Acutuaizar tablero vista

            //Revisamos movimientos del Boopy
            List<CambioBoop> cambios = motorDeReglas.Boopy(fila, col);

            //Realizamos el Boopy
            EjecutarCambiosBoop(cambios);

            // Buscamos lineas en el tablero y el tipo de linea
            ResultadoLinea resultado = motorDeReglas.RevisarLineas();

            // Revisamos el tipo de linea
            if (resultado.tipo == ResultadoLinea.Tipo.LINEA_GANADORA)
            {
                // Linea de gatos grandes, terminamos el juego
                juegoTerminado = true;
                ganador = jugadorActual.IdJugador;
            }
            else if (resultado.tipo == ResultadoLinea.Tipo.LINEA_NORMAL)
            {
                // Linea de gatos chicos, se hacen grandes
                PromoverGatitos(resultado.coords);
            }

            // cambiamos de turno
            // cambiarTurno();

            tablero.MostrarTablero();

            return true;
        }
                

        public void EjecutarCambiosBoop(List<CambioBoop> cambios)
        {
            foreach (var cambio in cambios)
            {
                // Siempre borramos la ficha de su origen
                tablero.BorrarGato(cambio.filaOrigen, cambio.colOrigen);

                if (cambio.fueraDelTablero)
                {
                    // Se cayó del tablero, devolver al contenedor del dueño
                    int tipoFicha = cambio.gatoEmpujado;
                    if (tipoFicha < 0) // Era del Jugador 1
                    {
                        jugador1.AgregarGatoAlContenedor(tipoFicha, 1);
                    }
                    else // Era del Jugador 2
                    {
                        jugador2.AgregarGatoAlContenedor(tipoFicha, 1);
                    }
                }
                else
                {
                    // Se movió a una nueva casilla
                    tablero.SetGato(cambio.gatoEmpujado, cambio.filaDestino, cambio.colDestino);
                }
            }
        }


        // Borra 3 gatitos del tablero y añade 3 gatotes al inventario del jugador
        public void PromoverGatitos(int[,] coords)
        {
            int gatoteToken = (turnoActual == Jugadores.JUGADOR1) ? jugador1.ValorGatote : jugador2.ValorGatote;

            // Borra los 3 gatitos del tablero
            for (int i = 0; i < 3; i++)
            {
                tablero.BorrarGato(coords[i, 0], coords[i, 1]);
            }

            // Añade 3 gatotes al contenedor del jugador
            if (turnoActual == Jugadores.JUGADOR1)
            {
                jugador1.AgregarGatoAlContenedor(gatoteToken, 3);
            }
            else
            {
                jugador2.AgregarGatoAlContenedor(gatoteToken, 3);
            }
        }

        public void cambiarTurno()
        {
            turnoActual = (turnoActual == Jugadores.JUGADOR1) ? Jugadores.JUGADOR2 : Jugadores.JUGADOR1;
            jugadorActual = (jugadorActual.IdJugador == (int)Jugadores.JUGADOR1) ? jugador2 : jugador1;
        }

        public bool JuegoTerminado()
        {
            return juegoTerminado;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}