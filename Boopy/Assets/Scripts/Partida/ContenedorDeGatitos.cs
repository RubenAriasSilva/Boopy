using UnityEngine;

namespace BoopyGame
{
public class ContenedorDeGatitos

{
    // Constantes
    private const int MAX_GATOS = 8;

    //Atributos
    private int valorGatito;
    private int valorGatote;

    private int cantGatosPequenos;
    private int cantGatosGrandes;

    //Constructor
    public ContenedorDeGatitos(int valorGatito = 0, int valorGatote = 0)
    {
        this.valorGatito = valorGatito;
        this.valorGatote = valorGatote;
        cantGatosPequenos = MAX_GATOS;
        cantGatosGrandes = 3;
    }

    // Constructor copia
    public ContenedorDeGatitos(ContenedorDeGatitos otro)
    {
        this.valorGatito = otro.valorGatito;
        this.valorGatote = otro.valorGatote;
        this.cantGatosPequenos = otro.cantGatosPequenos;
        this.cantGatosGrandes = otro.cantGatosGrandes;
    }

    // Propiedades
    public int ValorGatito => valorGatito;
    public int ValorGatote => valorGatote;
    public int CantGatosPequenos => cantGatosPequenos;
    public int CantGatosGrandes => cantGatosGrandes;
    public int TotalGatos => cantGatosPequenos + cantGatosGrandes;

    // Métodos
    public void QuitarGato(int tipo, int cantidad)
    {
        if (tipo == valorGatito)
        {
            cantGatosPequenos -= cantidad;
        }
        else if (tipo == valorGatote)
        {
            cantGatosGrandes -= cantidad;
        }
    }

    public bool AgregarGato(int tipo, int cantidad)
    {
        if (tipo == valorGatito)
        {
            cantGatosPequenos += cantidad;
        }
        else if (tipo == valorGatote)
        {
            cantGatosGrandes += cantidad;
        }

        if (TotalGatos > MAX_GATOS)
        {
            return false;
        }

        return true;
    }

    public void Clonar(ContenedorDeGatitos otro)
    {
        if (otro == null) return;

        cantGatosGrandes = otro.cantGatosGrandes;
        cantGatosPequenos = otro.cantGatosPequenos;
        valorGatito = otro.valorGatito;
        valorGatote = otro.valorGatote;      
    }



    public override string ToString()
    {
        return $"Gatitos pequeños: {cantGatosPequenos}, Gatotes: {cantGatosGrandes}, Total: {TotalGatos}";
    }
}
}