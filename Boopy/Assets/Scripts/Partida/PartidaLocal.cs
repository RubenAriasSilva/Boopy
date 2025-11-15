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
            
            // Lógica de selección
            if (jugadorActual != null)
            {
                // Convertimos el valor de la ficha para su respectivo jugador
                tipoFicha *= jugadorActual.ValorGatito;
                jugadorActual.SeleccionarGato(tipoFicha);
            }
        }

        public void ManejarMovimiento(int fila, int col)
        {
            // Inicia la corrutina de movimiento EN el controlador
            // (ya que esta clase no es un MonoBehaviour)
            controlador.StartCoroutine(RealizarMovimientoLocal(fila, col));
        }

        // ¡TODA TU LÓGICA DE CORRUTINA SE MUDÓ AQUÍ!
        private IEnumerator RealizarMovimientoLocal(int fila, int col)
        {
            controlador.SetEstaMoviendo(true);
            Jugador jugadorActual = controlador.GetJugadorActual();

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
                controlador.EjecutarCambiosBoop(cambios);
                Debug.Log("Ejecutando Boopy");
                controlador.tableroVista.ActualizarTableroVisual();
                yield return new WaitForSeconds(0.5f);
            }

            ResultadoLinea resultado = motorDeReglas.RevisarLineas();

            if (resultado.tipo == ResultadoLinea.Tipo.LINEA_GANADORA)
            {
                Debug.Log("Gano el jugador " + jugadorActual.IdJugador);
                controlador.SetJuegoTerminado(jugadorActual.IdJugador);
            }
            else if (resultado.tipo == ResultadoLinea.Tipo.LINEA_NORMAL)
            {
                Debug.Log("Gatos chicos se hacen grandes");
                controlador.PromoverGatitos(resultado.coords);
                controlador.tableroVista.ActualizarTableroVisual();
                yield return new WaitForSeconds(0.5f);
            }

            if (!controlador.JuegoTerminado())
            {
                controlador.cambiarTurno();
            }

            controlador.SetEstaMoviendo(false);
            Debug.Log("Final de realizar movimiento");
        }

        public void Actualizar()
        {
            // El modo local no necesita hacer nada en Update()
        }
    }
}