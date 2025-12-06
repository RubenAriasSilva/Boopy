using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoopyGame
{
    public class PartidaVsIA : IEstrategiaPartida
    {
        private PartidaControlador controlador;
        private TableroModelo tablero;
        private MotorDeReglas motorDeReglas;
        private PartidaVista partidaVista;
        private ControladorIA controladorIA;

        public void Iniciar(PartidaControlador controlador, TableroModelo tablero, MotorDeReglas motor, PartidaVista partidaVista)
        {
            this.controlador = controlador;
            this.tablero = tablero;
            this.motorDeReglas = motor;
            this.partidaVista = partidaVista;
            this.controladorIA = new ControladorIA(controlador, tablero, this);
        }
        
        // --- MÉTODO FALTANTE 1 ---
        public void ManejarSeleccionFicha(int tipoFicha)
        {
            Jugador jugadorActual = controlador.GetJugadorActual();
            
            // Esta función solo la usará el jugador humano. Si es el turno de la IA, no hacemos nada.
            if (jugadorActual.IdJugador == 2) return;
            
            // Lógica de selección para el jugador humano
            if (jugadorActual != null)
            {
                tipoFicha *= jugadorActual.ValorGatito;
                jugadorActual.SeleccionarGato(tipoFicha);
            }
        }

        // --- MÉTODO FALTANTE 2 ---
        public void ManejarMovimiento(int fila, int col)
        {
            // Esta función solo la usará el jugador humano.
            Jugador jugadorActual = controlador.GetJugadorActual();
            if (jugadorActual.IdJugador == 2) return;

            controlador.StartCoroutine(RealizarMovimientoLocal(fila, col));
        }

        public void Actualizar(){}

        public void SiguientePaso(){}

        public IEnumerator RealizarMovimientoLocal(int fila, int col)
        {            
            controlador.SetEstaMoviendo(true);
            Jugador jugadorActual = controlador.GetJugadorActual();
            Jugador jugadorGanador = null;
            int token;

            if (!jugadorActual.HaSeleccionadoGato)
            {
                // Este mensaje ahora puede aparecer si la IA intenta mover una ficha que no tiene.
                Debug.Log("No se ha seleccionado un gato (o la IA no tiene fichas de ese tipo)");
                controlador.SetEstaMoviendo(false);
                yield break;
            }

            if (!tablero.SetGato(jugadorActual.GatoSeleccionado, fila, col))
            {
                Debug.Log("Movimiento invalido");
                controlador.SetEstaMoviendo(false);
                yield break;
            }

            jugadorActual.QuitarGatoDelContenedor();
            jugadorActual.DeseleccionarGato();

            controlador.tableroVista.ActualizarTableroVisual();
            yield return new WaitForSeconds(0.4f);

            List<CambioBoop> cambios = motorDeReglas.Boopy(fila, col);
            if (cambios.Count > 0)
            {
                controlador.EjecutarCambiosBoopy(cambios);
                controlador.tableroVista.ActualizarTableroVisual();
                yield return new WaitForSeconds(0.5f);
            }

            if (jugadorActual.TodosLosGatosDentro())
            {
                Debug.Log("Gano el jugador " + jugadorActual.IdJugador + "Todos sus gatos dentro");
                controlador.SetJuegoTerminado(jugadorActual.IdJugador);
                yield return new WaitForSeconds(0.6f);
                partidaVista.TerminarPartida(jugadorActual.Nombre);
                yield break;
            }

            List<ResultadoLinea> resultados = motorDeReglas.RevisarLineas();
            if(resultados.Count > 0)
            {
                foreach(var resultado in resultados)
                {                    
                    if (resultado.tipo == ResultadoLinea.Tipo.LINEA_GANADORA)
                    {
                        token = tablero.GetGato(resultado.coords[0,0], resultado.coords[0,1]);
                        jugadorGanador = (token < 0) ? controlador.GetJugador1() : controlador.GetJugador2();
                        Debug.Log("Gano el jugador " + jugadorGanador.IdJugador);
                        controlador.SetJuegoTerminado(jugadorGanador.IdJugador);
                        yield return new WaitForSeconds(0.6f);
                        partidaVista.TerminarPartida(jugadorGanador.Nombre);
                        yield break;
                    }
                    else if (resultado.tipo == ResultadoLinea.Tipo.LINEA_NORMAL)
                    {
                        Debug.Log("Gatos chicos se hacen grandes");
                        controlador.PromoverGatitos(resultado.coords);
                        controlador.tableroVista.ActualizarTableroVisual();
                        yield return new WaitForSeconds(0.5f);
                    }   
                }                
            }

            controlador.ActualizarVista();

            if (!controlador.JuegoTerminado())
            {
                controlador.cambiarTurno();
            }

            controlador.SetEstaMoviendo(false);
            Debug.Log("Final de realizar movimiento");

            if (!controlador.JuegoTerminado() && controlador.GetJugadorActual().IdJugador == 2)
            {
                yield return new WaitForSeconds(1.0f); 
                Debug.Log("Turno de la IA");
                controladorIA.RealizarMovimientoAleatorio();
            }
        }
    }
}