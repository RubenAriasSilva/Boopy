using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoopyGame
{
    public class EstrategiaLocal : IEstrategiaPartida
    {
        private PartidaControlador controlador;
        private TableroModelo tablero;
        private MotorDeReglas motorDeReglas;

        // Constructor
        public void Iniciar(PartidaControlador controlador, TableroModelo tablero, MotorDeReglas motor)
        {
            this.controlador = controlador;
            this.tablero = tablero;
            this.motorDeReglas = motor;
        }

        public void ManejarSeleccionFicha(int tipoFicha)
        {
            Jugador jugadorActual = controlador.GetJugadorActual();
            
            // Logica de selección
            if (jugadorActual != null)
            {
                // Convertimos el valor de la ficha para su respectivo jugador
                tipoFicha *= jugadorActual.ValorGatito;
                jugadorActual.SeleccionarGato(tipoFicha);
            }
        }

        public void ManejarMovimiento(int fila, int col)
        {
            controlador.StartCoroutine(RealizarMovimientoLocal(fila, col));
        }

        // ¡TODA TU LÓGICA DE CORRUTINA SE MUDÓ AQUÍ!
        private IEnumerator RealizarMovimientoLocal(int fila, int col)
        {            
            
            controlador.SetEstaMoviendo(true);
            Jugador jugadorActual = controlador.GetJugadorActual();
            Jugador jugadorGanador = null;
            int token;

            if (!jugadorActual.HaSeleccionadoGato)
            {
                Debug.Log("No se ha seleccionado un gato");
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
                Debug.Log("Ejecutando Boopy");
                controlador.tableroVista.ActualizarTableroVisual();
                yield return new WaitForSeconds(0.5f);
            }

            // Si el jugador puso todos sus gatitos dentro del tablero, gana el juego
            if (jugadorActual.TodosLosGatosDentro())
            {
                Debug.Log("Gano el jugador " + jugadorActual.IdJugador + "Todos sus gatos dentro");
                controlador.SetJuegoTerminado(jugadorActual.IdJugador);
            }

            List<ResultadoLinea> resultados = motorDeReglas.RevisarLineas();
            if(resultados.Count > 0)
            {
                foreach(var resultado in resultados)
                {                    
                    if (resultado.tipo == ResultadoLinea.Tipo.LINEA_GANADORA) // Si el jugador hizo una linea de 3 gatotes, gana el juego
                    {
                        token = tablero.GetGato(resultado.coords[0,0], resultado.coords[0,1]);
                        jugadorGanador = (token < 0) ? controlador.GetJugador1() : controlador.GetJugador2();
                        Debug.Log("Gano el jugador " + jugadorGanador.IdJugador);
                        controlador.SetJuegoTerminado(jugadorGanador.IdJugador);
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

            // Si el juego termino, regresamos al menu principal
            // A futuro se puede agregar este metodo a un boton en la escena
            if (controlador.JuegoTerminado()) controlador.RegresarMenuPrincipal();

            if (!controlador.JuegoTerminado())
            {
                controlador.cambiarTurno();
            }

            controlador.SetEstaMoviendo(false);
            Debug.Log("Final de realizar movimiento");
        }

        public void SiguientePaso(){}

        public void Actualizar()
        {
            // El modo local no necesita hacer nada en Update()
        }
    }
}