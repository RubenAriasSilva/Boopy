using UnityEngine;
using System;
using System.Text;

namespace BoopyGame
{
    public class TableroModelo
    {
        // Constantes
        public const int TAMANIO_TABLERO = 6;

        // 0 = vacío, negativos = jugador 1, positivos = jugador 2
        private int[,] tablero = new int[TAMANIO_TABLERO, TAMANIO_TABLERO];

        // Constructor
        public TableroModelo() {}

        // Validaciones

        public bool DentroDelTablero(int fila, int col)
        {
            // Fila fuera de rango
            if (fila < 0 || fila >= TAMANIO_TABLERO) return false;
            
            // Columna fuera de rango
            if (col < 0 || col >= TAMANIO_TABLERO) return false;
            
            return true; // Posicion dentro del tablero
        }

        public bool HayUnGato(int fila, int col)
        {
            // Regresa verdadero si hay un gato, falso sino lo hay
            return tablero[fila, col] != 0;
        }

        public bool EspacioValido(int fila, int col)
        {
            // Es un espacio valido para poner una nueva ficha
            // Si esta dentro del tablero y no hay un gato
            return DentroDelTablero(fila, col) && !HayUnGato(fila, col);
        }

        // Setters y Getters
        public bool SetGato(int token, int fila, int col)
        {
            if (EspacioValido(fila, col))
            {
                tablero[fila, col] = token;
                return true;
            }
            return false;
        }

        public void BorrarGato(int fila, int col)
        {
            if (DentroDelTablero(fila, col))
                tablero[fila, col] = 0;
        }

        public int GetGato(int fila, int col)
        {
            return tablero[fila, col];
        }

        public int GetTamanioTablero()
        {
            return TAMANIO_TABLERO;
        }
        
        // Método para saber a quién pertenece la ficha
        public int GetPropietario(int fila, int col)
        {
            int valor = tablero[fila, col];
            if (valor == 0) return 0; // 0 = Nadie
            if (valor < 0) return 1;  // 1 = Jugador 1
            return 2;                 // 2 = Jugador 2
        }

        // Metodo para saber que tipo de ficha es (gatito o gatote)
        public int GetTipoFicha(int fila, int col)
        {
            // Devuelve el valor absoluto, que coincidira 
            // tus valorGatito = 1 y valorGatote = 2
            return Math.Abs(tablero[fila, col]);
        }

        // Mostrar toString del tablero
        public string MostrarTablero()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("  1 |  2 |  3 |  4 |  5 |  6");
            sb.AppendLine("-----------------------------");

            for (int i = 0; i < TAMANIO_TABLERO; i++)
            {
                for (int j = 0; j < TAMANIO_TABLERO; j++)
                {
                    if (tablero[i, j] == 0)
                        sb.Append("    ");
                    else
                        sb.Append(tablero[i, j].ToString().PadLeft(4));

                    sb.Append("|");

                    if (j == TAMANIO_TABLERO - 1)
                        sb.Append("  " + (i + 1));
                }

                sb.AppendLine();
                sb.AppendLine("-----------------------------");
            }

            return sb.ToString();
        }

    }
}
