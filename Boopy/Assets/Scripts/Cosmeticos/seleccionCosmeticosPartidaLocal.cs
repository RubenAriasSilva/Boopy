using UnityEngine;

public static class SeleccionDeCosmeticos
{
    public static string IdGatitoJugador1 { get; set; }
    public static string IdGatoJugador1 { get; set; }
    public static string IdGatitoJugador2 { get; set; }
    public static string IdGatoJugador2 { get; set; }

    public static void Limpiar()
    {
        IdGatitoJugador1 = null;
        IdGatoJugador1 = null;
        IdGatitoJugador2 = null;
        IdGatoJugador2 = null;
    }
}