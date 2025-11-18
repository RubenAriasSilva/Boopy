using UnityEngine;

namespace BoopyGame
{
    public class Jugador
    {
        // Atributos privados
        private int idJugador;
        private ContenedorDeGatitos contenedor;
        private bool haSeleccionadoGato;
        private int gatoSeleccionado; // 1 = pequeño, 2 = grande

        // Constructor
        public Jugador(int tipo, int valGatito, int valGatote)
        {
            idJugador = tipo;
            contenedor = new ContenedorDeGatitos(valGatito, valGatote);
            haSeleccionadoGato = false;
            gatoSeleccionado = 0;
        }

        // Propiedades (equivalentes a los getters)
        public int IdJugador => idJugador;
        public int ValorGatito => contenedor.ValorGatito;
        public int ValorGatote => contenedor.ValorGatote;
        public bool HaSeleccionadoGato => haSeleccionadoGato;
        public int GatoSeleccionado => gatoSeleccionado;
        public ContenedorDeGatitos Contenedor => contenedor;

        // Seleccionar un gato del contenedor
        public bool SeleccionarGato(int tipoGato)
        {
            if (tipoGato == contenedor.ValorGatito && contenedor.CantGatosPequenos > 0)
            {
                haSeleccionadoGato = true;
                gatoSeleccionado = contenedor.ValorGatito;
                return true;
            }
            else if (tipoGato == contenedor.ValorGatote && contenedor.CantGatosGrandes > 0)
            {
                haSeleccionadoGato = true;
                gatoSeleccionado = contenedor.ValorGatote;
                return true;
            }

            return false;
        }

        // Deseleccionar el gato
        public void DeseleccionarGato()
        {
            haSeleccionadoGato = false;
            gatoSeleccionado = 0;
        }

        // Quitar un gato del contenedor después de colocarlo
        public void QuitarGatoDelContenedor()
        {
            if (!haSeleccionadoGato) return;

            contenedor.QuitarGato(gatoSeleccionado, 1);            
        }

        // Agregar gatos al contenedor (por ejemplo, al retirarlos del tablero)
        public void AgregarGatoAlContenedor(int tipo, int cantidad)
        {
            contenedor.AgregarGato(tipo, cantidad);
        }

        public bool TodosLosGatosDentro()
        {
            if(contenedor.TotalGatos == 0) return true;

            return false;
        }

        // ToString opcional para depuración
        public override string ToString()
        {
            return $"Jugador {idJugador}: Gatitos X{contenedor.CantGatosPequenos} Gatotes X{contenedor.CantGatosGrandes}";
        }

    }
}
