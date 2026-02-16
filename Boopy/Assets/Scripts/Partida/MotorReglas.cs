using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using BoopyGame;
using System.Diagnostics;

public class MotorDeReglas
{
    private TableroModelo tablero;

    public MotorDeReglas(TableroModelo tableroRef)
    {
        tablero = tableroRef;
    }

    private bool EsLineaDeGatotes(int r1, int c1, int r2, int c2, int r3, int c3)
    {
        return Math.Abs(GetGato(r1, c1)) == 2 &&
               Math.Abs(GetGato(r2, c2)) == 2 &&
               Math.Abs(GetGato(r3, c3)) == 2;
    }

    private ResultadoLinea.Tipo TresEnFila(int token, int i, int j, ResultadoLinea resultado)
    {
        if (tablero.DentroDelTablero(i, j + 2))
        {
            if ((GetGato(i, j) * token) > 0 &&
                (GetGato(i, j + 1) * token) > 0 &&
                (GetGato(i, j + 2) * token) > 0)
            {
                resultado.coords[0, 0] = i; resultado.coords[0, 1] = j;
                resultado.coords[1, 0] = i; resultado.coords[1, 1] = j + 1;
                resultado.coords[2, 0] = i; resultado.coords[2, 1] = j + 2;

                return EsLineaDeGatotes(i, j, i, j + 1, i, j + 2)
                    ? ResultadoLinea.Tipo.LINEA_GANADORA
                    : ResultadoLinea.Tipo.LINEA_NORMAL;
            }
        }
        return ResultadoLinea.Tipo.NO_LINEA;
    }

    private ResultadoLinea.Tipo TresEnColumna(int token, int i, int j, ResultadoLinea resultado)
    {
        if (tablero.DentroDelTablero(i + 2, j))
        {
            if ((GetGato(i, j) * token) > 0 &&
                (GetGato(i + 1, j) * token) > 0 &&
                (GetGato(i + 2, j) * token) > 0)
            {
                resultado.coords[0, 0] = i; resultado.coords[0, 1] = j;
                resultado.coords[1, 0] = i + 1; resultado.coords[1, 1] = j;
                resultado.coords[2, 0] = i + 2; resultado.coords[2, 1] = j;

                return EsLineaDeGatotes(i, j, i + 1, j, i + 2, j)
                    ? ResultadoLinea.Tipo.LINEA_GANADORA
                    : ResultadoLinea.Tipo.LINEA_NORMAL;
            }
        }
        return ResultadoLinea.Tipo.NO_LINEA;
    }

    private ResultadoLinea.Tipo TresEnDiagonal(int token, int i, int j, ResultadoLinea resultado)
    {
        if (tablero.DentroDelTablero(i + 2, j - 2))
        {
            if ((GetGato(i, j) * token) > 0 &&
                (GetGato(i + 1, j - 1) * token) > 0 &&
                (GetGato(i + 2, j - 2) * token) > 0)
            {
                resultado.coords[0, 0] = i; resultado.coords[0, 1] = j;
                resultado.coords[1, 0] = i + 1; resultado.coords[1, 1] = j - 1;
                resultado.coords[2, 0] = i + 2; resultado.coords[2, 1] = j - 2;

                return EsLineaDeGatotes(i, j, i + 1, j - 1, i + 2, j - 2)
                    ? ResultadoLinea.Tipo.LINEA_GANADORA
                    : ResultadoLinea.Tipo.LINEA_NORMAL;
            }
        }

        if (tablero.DentroDelTablero(i + 2, j + 2))
        {
            if ((GetGato(i, j) * token) > 0 &&
                (GetGato(i + 1, j + 1) * token) > 0 &&
                (GetGato(i + 2, j + 2) * token) > 0)
            {
                resultado.coords[0, 0] = i; resultado.coords[0, 1] = j;
                resultado.coords[1, 0] = i + 1; resultado.coords[1, 1] = j + 1;
                resultado.coords[2, 0] = i + 2; resultado.coords[2, 1] = j + 2;

                return EsLineaDeGatotes(i, j, i + 1, j + 1, i + 2, j + 2)
                    ? ResultadoLinea.Tipo.LINEA_GANADORA
                    : ResultadoLinea.Tipo.LINEA_NORMAL;
            }
        }
        return ResultadoLinea.Tipo.NO_LINEA;
    }

    public List<CambioBoop> Boopy(int i, int j)
    {
        var cambios = new List<CambioBoop>();
        int gatoPuesto = GetGato(i, j);
        int gatoEmpujado;

        for (int row = i - 1; row <= i + 1; row++)
        {
            for (int col = j - 1; col <= j + 1; col++)
            {
                if (!tablero.DentroDelTablero(row, col)) continue;
                if (!tablero.HayUnGato(row, col)) continue;
                if (row == i && col == j) continue;

                gatoEmpujado = GetGato(row, col);
                if (Math.Abs(gatoPuesto) == 1 && Math.Abs(gatoEmpujado) == 2) continue;

                int r = row - (i - row);
                int c = col - (j - col);

                if (!tablero.DentroDelTablero(r, c))
                {
                    cambios.Add(new CambioBoop(row, col, -1, -1, true, gatoEmpujado));
                    continue;
                }

                if (tablero.HayUnGato(r, c)) continue;

                cambios.Add(new CambioBoop(row, col, r, c, false, gatoEmpujado));
            }
        }

        return cambios;
    }

    public int GetGato(int fila, int col)
    {
        return tablero.GetGato(fila, col);
    }

    public List<ResultadoLinea> RevisarLineas()
    {
        int tam = tablero.GetTamanioTablero();        
        var resultados = new List<ResultadoLinea>();        
        var resultado = new ResultadoLinea(); // tipo = NO_LINEA por defecto

        for (int row = 0; row < tam; row++)
        {
            for (int col = 0; col < tam; col++)
            {
                int token = GetGato(row, col);
                if (token == 0) continue;

                resultado.tipo = TresEnFila(token, row, col, resultado);
                if (resultado.tipo != ResultadoLinea.Tipo.NO_LINEA)
                {                                        
                    resultados.Add(new ResultadoLinea(resultado.tipo, (int[,])resultado.coords.Clone()));
                }
                resultado.tipo = TresEnColumna(token, row, col, resultado);
                if (resultado.tipo != ResultadoLinea.Tipo.NO_LINEA)
                {                    
                    resultados.Add(new ResultadoLinea(resultado.tipo, (int[,])resultado.coords.Clone()));
                }

                resultado.tipo = TresEnDiagonal(token, row, col, resultado);
                if (resultado.tipo != ResultadoLinea.Tipo.NO_LINEA)
                {                    
                    resultados.Add(new ResultadoLinea(resultado.tipo, (int[,])resultado.coords.Clone()));
                }
            }
        }
    
        return resultados;
    }

    public ResultadoLinea RevisarLineas2()
    {
        int tam = tablero.GetTamanioTablero();                
        var resultado = new ResultadoLinea(); // tipo = NO_LINEA por defecto

        for (int row = 0; row < tam; row++)
        {
            for (int col = 0; col < tam; col++)
            {
                int token = GetGato(row, col);
                if (token == 0) continue;

                resultado.tipo = TresEnFila(token, row, col, resultado);
                if (resultado.tipo != ResultadoLinea.Tipo.NO_LINEA) return resultado;
                
                resultado.tipo = TresEnColumna(token, row, col, resultado);
                if (resultado.tipo != ResultadoLinea.Tipo.NO_LINEA) return resultado;
                
                resultado.tipo = TresEnDiagonal(token, row, col, resultado);
                if (resultado.tipo != ResultadoLinea.Tipo.NO_LINEA) return resultado;                
            }
        }
    
        return resultado;
    }

    public string ToStringTablero()
    {
        return tablero.MostrarTablero();
    }
}
