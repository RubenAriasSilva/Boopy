using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace BoopyGame
{
    public class EstrategiaTutorial : IEstrategiaPartida
    {
        private PartidaControlador controlador;
        private TableroModelo tablero;
        private MotorDeReglas motor;
        private List<PasoTutorial> pasos;
        private int indicePasoActual = 0;
        private bool accionJugadorRealizada = false;

        public void Iniciar(PartidaControlador controlador, TableroModelo tablero, MotorDeReglas motor)
        {
            this.controlador = controlador;
            this.tablero = tablero;
            this.motor = motor;
            CrearPasos();
            EjecutarPaso();
        }
        
        private void CrearPasos()
        {
            pasos = new List<PasoTutorial>();

            // PASO 0: Bienvenida y Seleccion
            pasos.Add(new PasoTutorial 
            {
                mensajeInstruccion = "¡Bienvenido a Boopy! Para empezar, selecciona un Gatito de tu inventario",
                requiereSeleccionarFicha = true,
                tipoFichaEsperada = -1, // Asumiendo que J1 usa negativos (-1 gatito)
                accionDeConfiguracion = () => { 
                    // Asegurarnos de que el tablero esté limpio
                    controlador.LimpiarTableroParaTutorial(); 
                }
            });

            // PASO 1: Colocar gatito en el centro
            pasos.Add(new PasoTutorial 
            {
                mensajeInstruccion = "¡Muy bien! Ahora coloca tu Gatito en la cama (3,3)",
                requiereColocarFicha = true,
                filaEsperada = 3,
                colEsperada = 3,
                accionDeConfiguracion = () =>{
                    controlador.GetJugadorActual().SeleccionarGato(-1);
                    controlador.GetJugadorActual().QuitarGatoDelContenedor();
                    //controlador.GetJugadorActual().DeseleccionarGato();
                }
            });

            // PASO 2: Enseñar el Boop poniendo un gatito enemigo
            pasos.Add(new PasoTutorial 
            {
                mensajeInstruccion = "Mira!, Ha aparecido un gato rival y te ha empujado!",                
                accionDeConfiguracion = () => {
                    // Forzamos la aparición de un enemigo                    
                    controlador.tablero.SetGato(1, 1, 1); // Pone gatito enemigo en (2,2)                    
                    controlador.EjecutarCambiosBoopy(motor.Boopy(1,1));
                    controlador.ActualizarVista();
                }
            });

            // Paso 3: Jugador pone un gato en 3,3 de nuevo para empujar ambos gatos
            pasos.Add( new PasoTutorial
            {
                mensajeInstruccion = "Pon un gatito en la casilla 3,3 y ve lo que sucede!",
                requiereColocarFicha = true,
                filaEsperada = 3,
                colEsperada = 3,
                accionDeConfiguracion = () =>
                {
                    controlador.GetJugadorActual().SeleccionarGato(-1);
                    controlador.GetJugadorActual().QuitarGatoDelContenedor();
                    controlador.ActualizarVista();
                }
            });

            // Paso 4: Poner un gato enemigo en 6,3 para que este sea empujado fuera del tablero
            pasos.Add( new PasoTutorial
            {
                mensajeInstruccion = "Ahora que has aprendido que el Boopy afecta tus fichas y las de tu enemigo, probemos otra cosa!",
                accionDeConfiguracion = () =>
                {
                    controlador.LimpiarTableroParaTutorial();
                    controlador.tablero.SetGato(1, 5, 2);
                    controlador.ActualizarVista();
                }
            });
            // Paso 5: Jugador pone un gato en 5,3 para sacar al gato del tablero
            pasos.Add( new PasoTutorial
            {
                mensajeInstruccion = "Empuja al gatito enemigo fuea del tablero!, pon una gatito en 5,3",
                requiereSeleccionarFicha = true,
                tipoFichaEsperada = -1,
                requiereColocarFicha = true,
                filaEsperada = 5,
                colEsperada = 3,
                accionDeConfiguracion = () =>
                {
                    //controlador.EjecutarCambiosBoopy(motor.Boopy(4,2));
                    controlador.ActualizarVista();
                }
            });
            // Paso 6: Poner gato enemigo en 2,4
            pasos.Add( new PasoTutorial
            {
                mensajeInstruccion = "Han aparecido más gatos enemijos rodeando a uno de tus gatitos!",
                accionDeConfiguracion = () =>
                {
                    controlador.tablero.SetGato(1, 1, 3);
                    controlador.tablero.SetGato(1, 2, 4);
                    controlador.tablero.SetGato(-1, 2, 3);
                    controlador.ActualizarVista();
                }
            });            
            // Paso 7: Jugador pone un gato en 4,4 para ver el efecto del bloqueo con 2 gatos
            pasos.Add( new PasoTutorial
            {
                mensajeInstruccion = "Pon un gatito en la casilla 4,4 para ver que sucede cuando hay 2 gatos juntos",
                requiereSeleccionarFicha = true,
                tipoFichaEsperada = -1,
                requiereColocarFicha = true,
                filaEsperada = 3,
                colEsperada = 3,
                accionDeConfiguracion = () =>
                {
                    controlador.EjecutarCambiosBoopy(motor.Boopy(3,3));
                    controlador.ActualizarVista();
                }
            });
            // Paso 8: Poner gato enemigo en 2,5
            pasos.Add( new PasoTutorial
            {
                mensajeInstruccion = "",
                accionDeConfiguracion = () =>
                {
                    controlador.tablero.SetGato(1, 1, 4);
                    controlador.EjecutarCambiosBoopy(motor.Boopy(1,4));
                    controlador.ActualizarVista();
                }
            });
            // Paso 9: Jugador pone gato en 4,2 para hacer una linea de gatitos
            pasos.Add( new PasoTutorial
            {
                mensajeInstruccion = "Pon un gatito en la casilla 2,4 para hacer una linea de 3 gatitos",
                requiereSeleccionarFicha = true,
                tipoFichaEsperada = -1,
                requiereColocarFicha = true,
                filaEsperada = 2,
                colEsperada = 4,
                accionDeConfiguracion = () =>
                {
                    controlador.EjecutarCambiosBoopy(motor.Boopy(1,3));
                    controlador.ActualizarVista();
                }
            });
            // Paso 10: Seleccionar un gatote
            pasos.Add( new PasoTutorial
            {
                 mensajeInstruccion = "Ahora tienes 3 gatotes disponibles, selecciona uno de ellos",
                requiereSeleccionarFicha = true,
                tipoFichaEsperada = -2,                
            });
            // Paso 11: Jugador pone gatote en 4,3 para ver empuje
            pasos.Add( new PasoTutorial
            {
                mensajeInstruccion = "Pon el gatote en la casilla 4,3",
                requiereSeleccionarFicha = true,
                tipoFichaEsperada = -2,
                requiereColocarFicha = true,
                filaEsperada = 4,
                colEsperada = 3,
                accionDeConfiguracion = () =>
                {
                    controlador.EjecutarCambiosBoopy(motor.Boopy(3,2));
                    controlador.ActualizarVista();
                }
            });
            // Paso 12: Poner gatito en 4,4, para ver que un gatito no empuja a un gatote
            pasos.Add( new PasoTutorial
            {
                mensajeInstruccion = "Un gatito enemigo trato de empujar a tu gatote y fallo por la diferencia de peso",
                accionDeConfiguracion = () =>
                {
                    controlador.tablero.SetGato(1,3,3);
                    controlador.EjecutarCambiosBoopy(motor.Boopy(3,3));
                    controlador.ActualizarVista();
                }
            });
            // Paso 13: Borrar gatito en 4,4
            pasos.Add( new PasoTutorial
            {
                accionDeConfiguracion = () =>
                {
                    controlador.tablero.BorrarGato(3,3);
                }
            });
            // Paso 14: Poner un gatote, para ver que este si empuja gatotes
            pasos.Add( new PasoTutorial
            {
                mensajeInstruccion = "Un gatote enemigo empujo a tu gatote porque pesan lo mismo",
                accionDeConfiguracion = () =>
                {
                    controlador.tablero.SetGato(2,3,3);
                    controlador.EjecutarCambiosBoopy(motor.Boopy(3,3));
                    controlador.ActualizarVista();
                }
            });
            // Paso 15: Borrar tablero
            pasos.Add( new PasoTutorial
            {
                accionDeConfiguracion = () =>
                {
                    controlador.LimpiarTableroParaTutorial();
                }
            });
            // Paso 16: Poner gatotes en 3,4 y 4,4
            pasos.Add( new PasoTutorial
            {
                accionDeConfiguracion = () =>
                {
                    controlador.tablero.SetGato(2,2,3);
                    controlador.tablero.SetGato(2,3,3);
                }
            });
            // Paso 17: Jugador pone gatote en 5,4 para hacer linea ganadora
            pasos.Add( new PasoTutorial
            {
                mensajeInstruccion = "Pon un gatote en la casilla 5,4",
                requiereSeleccionarFicha = true,
                tipoFichaEsperada = -2,
                requiereColocarFicha = true,
                filaEsperada = 5,
                colEsperada = 4,
                accionDeConfiguracion = () =>
                {
                    controlador.EjecutarCambiosBoopy(motor.Boopy(4,3));
                    controlador.ActualizarVista();
                }
            });
            // Paso 18: Borrar tablero
            pasos.Add( new PasoTutorial
            {
                accionDeConfiguracion = () =>
                {
                    controlador.LimpiarTableroParaTutorial();
                }
            });
            // Paso 19: Poner 7 gatos en tablero
            pasos.Add( new PasoTutorial
            {
                accionDeConfiguracion = () =>
                {
                    controlador.tablero.SetGato(-1,2,2);
                    controlador.tablero.SetGato(-1,1,4);
                    controlador.tablero.SetGato(-2,2,5);
                    controlador.tablero.SetGato(-1,3,3);
                    controlador.tablero.SetGato(-2,3,5);
                    controlador.tablero.SetGato(-2,4,4);
                    controlador.tablero.SetGato(-1,2,0);
                    controlador.ActualizarVista();
                }
            });
            // Paso 20: Jugador pone el ultimo gatito en coord estrategica y gana con 8 gatos dentro
            pasos.Add( new PasoTutorial
            {
                mensajeInstruccion = "Pon un gatote en la casilla 3,4, esto no sacara ninguno de tus gatos y hará que ganes",
                requiereSeleccionarFicha = true,
                tipoFichaEsperada = -1,
                requiereColocarFicha = true,
                filaEsperada = 3,
                colEsperada = 4,
                accionDeConfiguracion = () =>
                {
                    controlador.EjecutarCambiosBoopy(motor.Boopy(3,4));
                    controlador.ActualizarVista();
                }
            });
        }

        public void SiguientePaso()
        {
            PasoTutorial paso = pasos[indicePasoActual];

            if((paso.requiereColocarFicha || paso.requiereSeleccionarFicha) && !accionJugadorRealizada){
                return;
            }
            indicePasoActual++;
            EjecutarPaso();
        }

        private void EjecutarPaso()
        {
            Debug.Log("Paso: " + indicePasoActual);
            PasoTutorial paso = pasos[indicePasoActual];
            
            controlador.MostrarMensaje(paso.mensajeInstruccion);
            
            if (paso.accionDeConfiguracion != null)
            {
                paso.accionDeConfiguracion.Invoke();                    
            }
            accionJugadorRealizada = false;
        }

        public void ManejarSeleccionFicha(int tipoFicha)
        {
            PasoTutorial paso = pasos[indicePasoActual];            

            if (paso.requiereSeleccionarFicha)
            {   
                tipoFicha *= controlador.GetJugadorActual().ValorGatito;                
                if (tipoFicha == paso.tipoFichaEsperada)
                {
                    controlador.GetJugadorActual().SeleccionarGato(tipoFicha);
                    accionJugadorRealizada = true;
                }
                else
                {
                    controlador.MostrarMensaje("Esa no es la ficha correcta. Intenta de nuevo.");                    
                }
            }
        }

        public void ManejarMovimiento(int fila, int col)
        {
            PasoTutorial paso = pasos[indicePasoActual];

            if (paso.requiereColocarFicha)
            {
                if (fila+1 == paso.filaEsperada && col+1 == paso.colEsperada)
                {
                    // ¡Movimiento Correcto!
                    // Iniciamos la corrutina visual en el controlador
                    controlador.StartCoroutine(RutinaMovimientoTutorial(fila, col));
                    accionJugadorRealizada = true;
                }
                else
                {
                    controlador.MostrarMensaje("¡Ahí no! Intenta en la posición indicada (" + paso.filaEsperada + "," + paso.colEsperada + ")");                    
                }
            }
        }
        
        private IEnumerator RutinaMovimientoTutorial(int fila, int col)
        {
            controlador.SetEstaMoviendo(true);
            Jugador jugadorActual = controlador.GetJugadorActual();

            // 1. Poner ficha y actualizar vista
            controlador.tablero.SetGato(jugadorActual.GatoSeleccionado, fila, col);
            jugadorActual.QuitarGatoDelContenedor(); // Opcional en tutorial: a veces quieres munición infinita
            
            controlador.ActualizarVista();
            yield return new WaitForSeconds(0.5f);

            // 2. Ejecutar Boop
            var cambios = controlador.motorDeReglas.Boopy(fila, col); // Asegurate de usar el nombre correcto de tu metodo
            if (cambios.Count > 0)
            {
                controlador.EjecutarCambiosBoopy(cambios);
                controlador.ActualizarVista();
                yield return new WaitForSeconds(0.5f);
            }

            // 3. Promoción (Si aplica al paso)
            var resultado = controlador.motorDeReglas.RevisarLineas(); // Asegurate de usar el nombre correcto
            if (resultado.tipo == ResultadoLinea.Tipo.LINEA_NORMAL)
            {
                controlador.PromoverGatitos(resultado.coords);
                controlador.ActualizarVista();
                yield return new WaitForSeconds(0.5f);
            }

            controlador.SetEstaMoviendo(false);            
        }

        private void FinalizarTutorial()
        {
            controlador.MostrarMensaje("¡Felicidades! Has completado el tutorial.");
            // Aquí podrías llamar a un método para volver al menú después de unos segundos
             controlador.StartCoroutine(SalirDespuesDeTiempo());
        }

        private IEnumerator SalirDespuesDeTiempo()
        {
             yield return new WaitForSeconds(3f);
             controlador.RegresarMenuPrincipal();
        }

        public void Actualizar()
        {
        }
    }
}



[System.Serializable]
public class PasoTutorial
{
    public string mensajeInstruccion; // Texto para la UI
    public bool requiereSeleccionarFicha; // ¿El objetivo es tocar el inventario?
    public int tipoFichaEsperada; // Si requiere selecciopnar una ficha, ¿cual?
    public bool requiereColocarFicha; // ¿El objetivo es tocar el tablero?
    public int filaEsperada, colEsperada; // Coordenadas
    public System.Action accionDeConfiguracion; // Codigo para preparar el tablero antes del paso
}