using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Modelo de Cosmético", menuName = "Cosmetics/Modelo de Cosmético")]
public class cosmeticosModelo : ScriptableObject
{
    [Header("Información Básica")]
    public string id; // ID único, ej: "sombrero_vikingo_01"

    [Header("Nombres por Idioma")]
    // Índice 0: Español, Índice 1: Inglés, etc.
    public string[] nombresPorIdioma;

    [Header("Recursos Visuales")]
    public Sprite iconoUI; // El icono para la tabla
    public GameObject prefab3DRojo; // El prefab 3D que se instancia en el personaje
    public GameObject prefab3DAzul; // El prefab 3D que se instancia en el personaje
}