using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace BoopyGame
{
    public class PartidaControlador : MonoBehaviour
    {
        [Header("Vistas")]
        public TableroVista tableroVista;

        public TableroModelo tablero { get; private set; }
        public MotorDeReglas motorDeReglas { get; private set; }
        public Jugador jugador1 { get; private set; }
        public Jugador jugador2 { get; private set; }

        private IEstrategiaPartida estrategiaActual;
        private Jugador jugadorActual;
        private Jugadores turnoActual;
        private bool juegoTerminado = false;
        private bool estaMoviendo = false;
        private int ganador = 0;

        // Enumeracion para los jugadores
        private enum Jugadores { JUGADOR1 = 1, JUGADOR2 = 2 }
                
        public void IniciarPartida(ModoDeJuego modo)
        {
            // Inicialización de los objetos
            tablero = new TableroModelo();
            motorDeReglas = new MotorDeReglas(tablero);
            jugador1 = new Jugador((int)Jugadores.JUGADOR1, -1, -2);
            jugador2 = new Jugador((int)Jugadores.JUGADOR2, 1, 2);
            jugadorActual = jugador1;
            turnoActual = Jugadores.JUGADOR1;
            juegoTerminado = false;

            // Conectar la Vista
            if (tableroVista == null)
            {
                tableroVista = FindFirstObjectByType<TableroVista>();
            }
            tableroVista.Inicializar(this);
            tableroVista.copiarTablero(tablero);

            // SELECCIONAR E INICIAR LA ESTRATEGIA
            switch (modo)
            {
                case ModoDeJuego.Local:
                    estrategiaActual = new EstrategiaLocal();
                    break;
                case ModoDeJuego.Tutorial:
                    // estrategiaActual = new EstrategiaTutorial();
                    break;
                case ModoDeJuego.VsIA:
                    // estrategiaActual = new EstrategiaVsIA();
                    break;
            }
            
            // Inicia la estrategia seleccionada
            estrategiaActual.Iniciar(this, tablero, motorDeReglas);
            Debug.Log($"Partida iniciada en modo: {modo}");
        }

        void Update()
        {
            if (estrategiaActual != null && !juegoTerminado)
            {
                estrategiaActual.Actualizar();
            }
        }

        public void ClickEnCasilla(int fila, int col)
        {
            if (estrategiaActual != null && !estaMoviendo && !juegoTerminado)
            {
                estrategiaActual.ManejarMovimiento(fila, col);
            }
        }

        public void ClickEnContenedor(int tipoFicha)
        {
            if (estrategiaActual != null && !estaMoviendo && !juegoTerminado)
            {
                estrategiaActual.ManejarSeleccionFicha(tipoFicha);
            }
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

        public void IntentarMovimiento(int fila, int col)
        {
            if(estaMoviendo) return;
            Debug.Log("Jugador Actual: " + jugadorActual.IdJugador);
            Debug.Log(jugador1.ToString());
            Debug.Log(jugador2.ToString());
            StartCoroutine(RealizarMovimiento(fila, col));
        }

        private IEnumerator RealizarMovimiento(int fila, int col)
        {
            estaMoviendo = true;

            if (!jugadorActual.HaSeleccionadoGato)
            {
                Debug.Log("No se ha seleccionado un gato");
                estaMoviendo = false;
                yield break;
            }
            
            //Revisamos si el movimiento es valido
            if (!tablero.SetGato(jugadorActual.GatoSeleccionado, fila, col))
            {
                Debug.Log("Movimiento invalido");
                estaMoviendo = false;
                yield break;
            }

            jugadorActual.QuitarGatoDelContenedor();
            jugadorActual.DeseleccionarGato();
            
            tableroVista.ActualizarTableroVisual();

            //Delay
            yield return new WaitForSeconds(0.4f);

            //Revisamos movimientos del Boopy
            List<CambioBoop> cambios = motorDeReglas.Boopy(fila, col);
            if(cambios.Count > 0)
            {
                //Realizamos el Boopy
                EjecutarCambiosBoop(cambios);
                Debug.Log("Ejecutando Boopy");

                tableroVista.ActualizarTableroVisual();
                
                // Otra pausa
                yield return new WaitForSeconds(0.5f);
            }

            // Buscamos lineas en el tablero y el tipo de linea
            ResultadoLinea resultado = motorDeReglas.RevisarLineas();

            // Revisamos el tipo de linea
            if (resultado.tipo == ResultadoLinea.Tipo.LINEA_GANADORA)
            {
                // Linea de gatos grandes, terminamos el juego
                Debug.Log("Gano el jugador " + idJugadorAcutal());
                juegoTerminado = true;
                ganador = jugadorActual.IdJugador;
            }
            else if (resultado.tipo == ResultadoLinea.Tipo.LINEA_NORMAL)
            {
                // Linea de gatos chicos, se hacen grandes
                Debug.Log("Gatos chicos se hacen grandes");
                PromoverGatitos(resultado.coords);

                tableroVista.ActualizarTableroVisual();
                yield return new WaitForSeconds(0.5f);
            }

            if (!juegoTerminado)
            {
                // cambiamos de turno
                cambiarTurno();    
            }

            estaMoviendo = false;

            Debug.Log("Final de realizar movimiento");
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


        public Jugador GetJugadorActual() => jugadorActual;
        public bool JuegoTerminado() => juegoTerminado;
        public int idJugadorAcutal() => jugadorActual.IdJugador;
        public void SetEstaMoviendo(bool valor) => estaMoviendo = valor;
        public void SetJuegoTerminado(int idGanador)
        {
            juegoTerminado = true;
            ganador = idGanador;
        }
    }
}