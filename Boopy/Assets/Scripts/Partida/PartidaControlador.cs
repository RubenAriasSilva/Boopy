using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
namespace BoopyGame
{
    public class PartidaControlador : MonoBehaviour
    {
        [Header("Vistas")]
        public TableroVista tableroVista;
        public ContenedorVista contenedorVista;
        public PartidaTutorialVista tutorialVista;

        public GameObject botonContinuar;

        public GameObject contenedorJugador1;
        public GameObject contenedorJugador2;

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

        public GameObject ajustes;

        ModoDeJuego modo;

        void Start()
        {
            //Recuperamos la elección guardada en el GameManager
            modo = GameManager.Instance.ModoSeleccionado;
            botonContinuar.SetActive(false);
            // Nos auto-configuramos con esa estrategia
            IniciarPartida(modo);
        }

        // Enumeracion para los jugadores
        private enum Jugadores { JUGADOR1 = 1, JUGADOR2 = 2 }
                
        public void IniciarPartida(ModoDeJuego modo)
        {
            ConfiguracionPartida confPart = GameManager.Instance.confPartidaActual;
            string nombreJ1 = confPart.nombreJugador1;
            string nombreJ2 = confPart.nombreJugador2;
            // Inicialización de los objetos
            tablero = new TableroModelo();
            motorDeReglas = new MotorDeReglas(tablero);
            jugador1 = new Jugador(nombreJ1, (int)Jugadores.JUGADOR1, -1, -2);
            jugador2 = new Jugador(nombreJ2, (int)Jugadores.JUGADOR2, 1, 2);
            jugadorActual = jugador1;
            turnoActual = Jugadores.JUGADOR1;
            contenedorJugador1.SetActive(true);
            contenedorJugador2.SetActive(false);
            juegoTerminado = false;

            // Conectar la Vista
            if (tableroVista == null)
            {
                tableroVista = FindFirstObjectByType<TableroVista>();
            }
            tableroVista.Inicializar(this);
            tableroVista.copiarTablero(tablero);

            // Conectar la Vista
            if (contenedorVista == null)
            {
                contenedorVista = FindFirstObjectByType<ContenedorVista>();
            }
            contenedorVista.Inicializar(this, jugador1, jugador2);

            // SELECCIONAR E INICIAR LA ESTRATEGIA
            switch (modo)
            {
                case ModoDeJuego.Local:
                    estrategiaActual = new EstrategiaLocal();
                    break;
                case ModoDeJuego.Tutorial:
                    botonContinuar.SetActive(true);
                    estrategiaActual = new EstrategiaTutorial();
                    if(tutorialVista == null)
                    {
                        tutorialVista = FindFirstObjectByType<PartidaTutorialVista>();
                    }
                    tutorialVista.Inicializar(this);
                    break;
                case ModoDeJuego.VsIA:
                    // estrategiaActual = new EstrategiaVsIA();
                    break;                
            }
            
            // Inicia la estrategia seleccionada
            estrategiaActual.Iniciar(this, tablero, motorDeReglas);
            Debug.Log($"Partida iniciada en modo: {modo}");
            ActualizarVista();
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
            if (estrategiaActual != null && !estaMoviendo && !juegoTerminado && jugadorActual.HaSeleccionadoGato)
            {
                estrategiaActual.ManejarMovimiento(fila, col);
            }            
        }

        public void ClickEnContenedor(int idJugador ,int tipoFicha)
        {
            if((int)turnoActual != idJugador) {
                Debug.Log("No es turno de este jugador");
                return;
            }

            if (estrategiaActual != null && !estaMoviendo && !juegoTerminado)
            {
                estrategiaActual.ManejarSeleccionFicha(tipoFicha);
            }
        }

        public void EjecutarCambiosBoopy(List<CambioBoop> cambios)
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
            ActualizarVista();
        }


        // Borra 3 gatitos del tablero y añade 3 gatotes al inventario del jugador
        public void PromoverGatitos(int[,] coords)
        {
            // Obtenemos el valor de las primeras coordenadas del primer gato
            //Debug.Log("Coords" + coords[0,0] + "," +coords[0,1]);
            int gatoToken = tablero.GetGato(coords[0,0], coords[0,1]);
            int gatoteToken = 0;

            // Borra los 3 gatitos del tablero
            for (int i = 0; i < 3; i++)
            {
                tablero.BorrarGato(coords[i, 0], coords[i, 1]);
            }

            // Añade 3 gatotes al contenedor del jugador
            if (gatoToken < 0)
            {
                gatoteToken = jugador1.ValorGatote;
                jugador1.AgregarGatoAlContenedor(gatoteToken, 3);
            }
            else if (gatoToken > 0)
            {
                gatoteToken = jugador2.ValorGatote;
                jugador2.AgregarGatoAlContenedor(gatoteToken, 3);
            }
            ActualizarVista();
        }

        public void cambiarTurno()
        {
            turnoActual = (turnoActual == Jugadores.JUGADOR1) ? Jugadores.JUGADOR2 : Jugadores.JUGADOR1;
            jugadorActual = (jugadorActual.IdJugador == (int)Jugadores.JUGADOR1) ? jugador2 : jugador1;

            if(jugadorActual.IdJugador == (int)Jugadores.JUGADOR1){
                contenedorJugador1.SetActive(true);
                contenedorJugador2.SetActive(false);
            } else {
                contenedorJugador1.SetActive(false);
                contenedorJugador2.SetActive(true);
            }
        }

        public void RegresarMenuPrincipal()
        {
            Debug.Log("Saliendo de la partida...");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CargarMenuPrincipal();
            }
            else
            {             
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }

        public Jugador GetJugadorActual() => jugadorActual;
        public Jugador GetJugador1() => jugador1;
        public Jugador GetJugador2() => jugador2;
        public bool JuegoTerminado() => juegoTerminado;
        public int idJugadorAcutal() => jugadorActual.IdJugador;
        public void SetEstaMoviendo(bool valor) => estaMoviendo = valor;
        public void SetJuegoTerminado(int idGanador)
        {
            juegoTerminado = true;
            ganador = idGanador;
        }


        // Metodos para partida tutorial
        public void LimpiarTableroParaTutorial()
        {            
            for(int i=0; i<6; i++)
                for(int j=0; j<6; j++)
                    tablero.BorrarGato(i, j);
            
            ActualizarVista();
        }

        public void SiguientePasoTutorial()
        {
            estrategiaActual.SiguientePaso();
        }

        // Método para comunicarse con la UI (Asumiendo que tienes un texto en pantalla)
        /*
        [Header("UI")]
        public TMPro.TextMeshProUGUI textoInstrucciones; // Asignar en Inspector

        public void MostrarMensajeUI(string mensaje)
        {
            if(textoInstrucciones != null)
                textoInstrucciones.text = mensaje;
            
            Debug.Log("UI: " + mensaje);
        }
        */

        public void MostrarMensaje(string msg)
        {
            Debug.Log(msg);
        }

        // Helper para forzar actualización visual
        public void ActualizarVista()
        {
            tableroVista.ActualizarTableroVisual();
            contenedorVista.actualizarcontenedores();
        }

        public void AbrirAjustes()
        {
            ajustes.SetActive(true);
        }

        public void CerrarAjustes()
        {
            ajustes.SetActive(false);
        }
    }
}