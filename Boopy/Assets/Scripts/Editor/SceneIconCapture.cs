using UnityEngine;
using UnityEditor;
using System.IO;

public class SceneIconCapture
{
    // Añade una opción en el menú de Unity
    [MenuItem("Herramientas/Capturar Icono desde Escena")]
    private static void CaptureIconFromScene()
    {
        // 1. Obtenemos el prefab seleccionado en la ventana de Project
        GameObject selectedPrefab = Selection.activeObject as GameObject;
        if (selectedPrefab == null)
        {
            EditorUtility.DisplayDialog("Error", "Por favor, selecciona un prefab en la ventana de Project.", "OK");
            return;
        }

        // 2. Buscamos la cámara con el tag que definimos
        GameObject cameraObj = GameObject.FindWithTag("IconCaptureCamera");
        if (cameraObj == null)
        {
            EditorUtility.DisplayDialog("Error", "No se encontró una cámara con el tag 'IconCaptureCamera' en la escena.", "OK");
            return;
        }
        Camera camera = cameraObj.GetComponent<Camera>();
        if (camera == null)
        {
            EditorUtility.DisplayDialog("Error", "El objeto con el tag 'IconCaptureCamera' no tiene un componente Camera.", "OK");
            return;
        }

        // 3. Configuramos la textura de renderizado
        int iconSize = 512; // Puedes cambiar este tamaño (256, 512, 1024...)
        RenderTexture renderTexture = new RenderTexture(iconSize, iconSize, 24);
        camera.targetTexture = renderTexture;

        // 4. Renderizamos la vista de la cámara
        camera.Render();
        RenderTexture.active = renderTexture;

        // 5. Creamos una textura 2D y leemos los píxeles
        Texture2D iconTexture = new Texture2D(iconSize, iconSize, TextureFormat.RGBA32, false);
        iconTexture.ReadPixels(new Rect(0, 0, iconSize, iconSize), 0, 0);
        iconTexture.Apply();
        
        // 6. Limpiamos para no afectar a la escena
        RenderTexture.active = null; // ¡Muy importante!
        camera.targetTexture = null; // ¡Muy importante!
        Object.DestroyImmediate(renderTexture); // Destruimos la textura temporal

        // 7. Codificamos a PNG y guardamos el archivo
        byte[] bytes = iconTexture.EncodeToPNG();
        string savePath = "Assets/GeneratedIcons"; // Carpeta donde se guardarán los iconos
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        string filePath = Path.Combine(savePath, $"{selectedPrefab.name}_icon.png");
        File.WriteAllBytes(filePath, bytes);

        // 8. Refrescamos la base de datos de assets para que aparezca el nuevo archivo
        AssetDatabase.Refresh();

        // 9. Mostramos un mensaje de éxito
        EditorUtility.DisplayDialog("Éxito", $"Icono guardado en: {filePath}", "OK");
    }
}