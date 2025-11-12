using UnityEngine;
using UnityEngine.SceneManagement;

public class Tablero : MonoBehaviour
{

    public GameObject gatitoPrefab;
    public GameObject gatoPrefab;
    public bool tipoGato;

    void Start()
    {

    }

    public void Contenedor(bool estado)
    {
        tipoGato = estado;
    }

    public void SeleccionarCasilla(string nombre)
    {
        // Separa algo como "b3_4" -> ["b3", "4"]
        string[] partes = nombre.Split('_');

        int fila = int.Parse(partes[0].Substring(1)); // de "b3" toma "3"
        int columna = int.Parse(partes[1]);           // de "4" toma "4"

        switch (tipoGato)
        {
            case false:
                switch (fila)
                {
                    case 1:
                        switch (columna)
                        {
                            case 1: Instantiate(gatitoPrefab, Posiciones.g1_1, Posiciones.gRot); break;
                            case 2: Instantiate(gatitoPrefab, Posiciones.g1_2, Posiciones.gRot); break;
                            case 3: Instantiate(gatitoPrefab, Posiciones.g1_3, Posiciones.gRot); break;
                            case 4: Instantiate(gatitoPrefab, Posiciones.g1_4, Posiciones.gRot); break;
                            case 5: Instantiate(gatitoPrefab, Posiciones.g1_5, Posiciones.gRot); break;
                            case 6: Instantiate(gatitoPrefab, Posiciones.g1_6, Posiciones.gRot); break;
                        }
                        break;

                    case 2:
                        switch (columna)
                        {
                            case 1: Instantiate(gatitoPrefab, Posiciones.g2_1, Posiciones.gRot); break;
                            case 2: Instantiate(gatitoPrefab, Posiciones.g2_2, Posiciones.gRot); break;
                            case 3: Instantiate(gatitoPrefab, Posiciones.g2_3, Posiciones.gRot); break;
                            case 4: Instantiate(gatitoPrefab, Posiciones.g2_4, Posiciones.gRot); break;
                            case 5: Instantiate(gatitoPrefab, Posiciones.g2_5, Posiciones.gRot); break;
                            case 6: Instantiate(gatitoPrefab, Posiciones.g2_6, Posiciones.gRot); break;
                        }
                        break;

                    case 3:
                        switch (columna)
                        {
                            case 1: Instantiate(gatitoPrefab, Posiciones.g3_1, Posiciones.gRot); break;
                            case 2: Instantiate(gatitoPrefab, Posiciones.g3_2, Posiciones.gRot); break;
                            case 3: Instantiate(gatitoPrefab, Posiciones.g3_3, Posiciones.gRot); break;
                            case 4: Instantiate(gatitoPrefab, Posiciones.g3_4, Posiciones.gRot); break;
                            case 5: Instantiate(gatitoPrefab, Posiciones.g3_5, Posiciones.gRot); break;
                            case 6: Instantiate(gatitoPrefab, Posiciones.g3_6, Posiciones.gRot); break;
                        }
                        break;

                    case 4:
                        switch (columna)
                        {
                            case 1: Instantiate(gatitoPrefab, Posiciones.g4_1, Posiciones.gRot); break;
                            case 2: Instantiate(gatitoPrefab, Posiciones.g4_2, Posiciones.gRot); break;
                            case 3: Instantiate(gatitoPrefab, Posiciones.g4_3, Posiciones.gRot); break;
                            case 4: Instantiate(gatitoPrefab, Posiciones.g4_4, Posiciones.gRot); break;
                            case 5: Instantiate(gatitoPrefab, Posiciones.g4_5, Posiciones.gRot); break;
                            case 6: Instantiate(gatitoPrefab, Posiciones.g4_6, Posiciones.gRot); break;
                        }
                        break;

                    case 5:
                        switch (columna)
                        {
                            case 1: Instantiate(gatitoPrefab, Posiciones.g5_1, Posiciones.gRot); break;
                            case 2: Instantiate(gatitoPrefab, Posiciones.g5_2, Posiciones.gRot); break;
                            case 3: Instantiate(gatitoPrefab, Posiciones.g5_3, Posiciones.gRot); break;
                            case 4: Instantiate(gatitoPrefab, Posiciones.g5_4, Posiciones.gRot); break;
                            case 5: Instantiate(gatitoPrefab, Posiciones.g5_5, Posiciones.gRot); break;
                            case 6: Instantiate(gatitoPrefab, Posiciones.g5_6, Posiciones.gRot); break;
                        }
                        break;

                    case 6:
                        switch (columna)
                        {
                            case 1: Instantiate(gatitoPrefab, Posiciones.g6_1, Posiciones.gRot); break;
                            case 2: Instantiate(gatitoPrefab, Posiciones.g6_2, Posiciones.gRot); break;
                            case 3: Instantiate(gatitoPrefab, Posiciones.g6_3, Posiciones.gRot); break;
                            case 4: Instantiate(gatitoPrefab, Posiciones.g6_4, Posiciones.gRot); break;
                            case 5: Instantiate(gatitoPrefab, Posiciones.g6_5, Posiciones.gRot); break;
                            case 6: Instantiate(gatitoPrefab, Posiciones.g6_6, Posiciones.gRot); break;
                        }
                        break;
                }
                break;

            case true:
                switch (fila)
                {
                    case 1:
                        switch (columna)
                        {
                            case 1: Instantiate(gatoPrefab, Posiciones.G1_1, Posiciones.GRot); break;
                            case 2: Instantiate(gatoPrefab, Posiciones.G1_2, Posiciones.GRot); break;
                            case 3: Instantiate(gatoPrefab, Posiciones.G1_3, Posiciones.GRot); break;
                            case 4: Instantiate(gatoPrefab, Posiciones.G1_4, Posiciones.GRot); break;
                            case 5: Instantiate(gatoPrefab, Posiciones.G1_5, Posiciones.GRot); break;
                            case 6: Instantiate(gatoPrefab, Posiciones.G1_6, Posiciones.GRot); break;
                        }
                        break;

                    case 2:
                        switch (columna)
                        {
                            case 1: Instantiate(gatoPrefab, Posiciones.G2_1, Posiciones.GRot); break;
                            case 2: Instantiate(gatoPrefab, Posiciones.G2_2, Posiciones.GRot); break;
                            case 3: Instantiate(gatoPrefab, Posiciones.G2_3, Posiciones.GRot); break;
                            case 4: Instantiate(gatoPrefab, Posiciones.G2_4, Posiciones.GRot); break;
                            case 5: Instantiate(gatoPrefab, Posiciones.G2_5, Posiciones.GRot); break;
                            case 6: Instantiate(gatoPrefab, Posiciones.G2_6, Posiciones.GRot); break;
                        }
                        break;

                    case 3:
                        switch (columna)
                        {
                            case 1: Instantiate(gatoPrefab, Posiciones.G3_1, Posiciones.GRot); break;
                            case 2: Instantiate(gatoPrefab, Posiciones.G3_2, Posiciones.GRot); break;
                            case 3: Instantiate(gatoPrefab, Posiciones.G3_3, Posiciones.GRot); break;
                            case 4: Instantiate(gatoPrefab, Posiciones.G3_4, Posiciones.GRot); break;
                            case 5: Instantiate(gatoPrefab, Posiciones.G3_5, Posiciones.GRot); break;
                            case 6: Instantiate(gatoPrefab, Posiciones.G3_6, Posiciones.GRot); break;
                        }
                        break;

                    case 4:
                        switch (columna)
                        {
                            case 1: Instantiate(gatoPrefab, Posiciones.G4_1, Posiciones.GRot); break;
                            case 2: Instantiate(gatoPrefab, Posiciones.G4_2, Posiciones.GRot); break;
                            case 3: Instantiate(gatoPrefab, Posiciones.G4_3, Posiciones.GRot); break;
                            case 4: Instantiate(gatoPrefab, Posiciones.G4_4, Posiciones.GRot); break;
                            case 5: Instantiate(gatoPrefab, Posiciones.G4_5, Posiciones.GRot); break;
                            case 6: Instantiate(gatoPrefab, Posiciones.G4_6, Posiciones.GRot); break;
                        }
                        break;

                    case 5:
                        switch (columna)
                        {
                            case 1: Instantiate(gatoPrefab, Posiciones.G5_1, Posiciones.GRot); break;
                            case 2: Instantiate(gatoPrefab, Posiciones.G5_2, Posiciones.GRot); break;
                            case 3: Instantiate(gatoPrefab, Posiciones.G5_3, Posiciones.GRot); break;
                            case 4: Instantiate(gatoPrefab, Posiciones.G5_4, Posiciones.GRot); break;
                            case 5: Instantiate(gatoPrefab, Posiciones.G5_5, Posiciones.GRot); break;
                            case 6: Instantiate(gatoPrefab, Posiciones.G5_6, Posiciones.GRot); break;
                        }
                        break;

                    case 6:
                        switch (columna)
                        {
                            case 1: Instantiate(gatoPrefab, Posiciones.G6_1, Posiciones.GRot); break;
                            case 2: Instantiate(gatoPrefab, Posiciones.G6_2, Posiciones.GRot); break;
                            case 3: Instantiate(gatoPrefab, Posiciones.G6_3, Posiciones.GRot); break;
                            case 4: Instantiate(gatoPrefab, Posiciones.G6_4, Posiciones.GRot); break;
                            case 5: Instantiate(gatoPrefab, Posiciones.G6_5, Posiciones.GRot); break;
                            case 6: Instantiate(gatoPrefab, Posiciones.G6_6, Posiciones.GRot); break;
                        }
                        break;
                }
                break;
        }

        
    }

}

public static class Posiciones
{
    // Gatito
    // Rotacion x y z
    public static Quaternion gRot = Quaternion.Euler(-90f, 90f, 26.797f);
    // Pocicion x y z
    public static Vector3 g1_1 = new Vector3(-4.32f, 1.4f, 14.28f);
    public static Vector3 g1_2 = new Vector3(-2.30f, 1.4f, 14.16f);
    public static Vector3 g1_3 = new Vector3(-0.37f, 1.4f, 14f);
    public static Vector3 g1_4 = new Vector3(1.62f, 1.4f, 13.89f);
    public static Vector3 g1_5 = new Vector3(3.54f, 1.4f, 13.78f);
    public static Vector3 g1_6 = new Vector3(5.45f, 1.4f, 13.62f);
    public static Vector3 g2_1 = new Vector3(-4.37f, 1.4f, 12.34f);
    public static Vector3 g2_2 = new Vector3(-2.41f, 1.4f, 12.25f);
    public static Vector3 g2_3 = new Vector3(-0.52f, 1.4f, 12.1f);
    public static Vector3 g2_4 = new Vector3(1.42f, 1.4f, 11.91f);
    public static Vector3 g2_5 = new Vector3(3.38f, 1.4f, 11.76f);
    public static Vector3 g2_6 = new Vector3(5.27f, 1.4f, 11.63f);
    public static Vector3 g3_1 = new Vector3(-4.6f, 1.4f, 10.27f);
    public static Vector3 g3_2 = new Vector3(-2.62f, 1.4f, 10.23f);
    public static Vector3 g3_3 = new Vector3(-0.69f, 1.4f, 10.06f);
    public static Vector3 g3_4 = new Vector3(1.28f, 1.4f, 9.94f);
    public static Vector3 g3_5 = new Vector3(3.24f, 1.4f, 9.81f);
    public static Vector3 g3_6 = new Vector3(5.18f, 1.4f, 9.68f);
    public static Vector3 g4_1 = new Vector3(-4.74f, 1.4f, 8.29f);
    public static Vector3 g4_2 = new Vector3(-2.71f, 1.4f, 8.26f);
    public static Vector3 g4_3 = new Vector3(-0.87f, 1.4f, 8.07f);
    public static Vector3 g4_4 = new Vector3(1.15f, 1.4f, 7.95f);
    public static Vector3 g4_5 = new Vector3(3.08f, 1.4f, 7.86f);
    public static Vector3 g4_6 = new Vector3(5.04f, 1.4f, 7.74f);
    public static Vector3 g5_1 = new Vector3(-4.8f, 1.4f, 6.39f);
    public static Vector3 g5_2 = new Vector3(-2.92f, 1.4f, 6.26f);
    public static Vector3 g5_3 = new Vector3(-0.93f, 1.4f, 6.09f);
    public static Vector3 g5_4 = new Vector3(1.04f, 1.4f, 5.96f);
    public static Vector3 g5_5 = new Vector3(2.98f, 1.4f, 5.79f);
    public static Vector3 g5_6 = new Vector3(4.85f, 1.4f, 5.64f);
    public static Vector3 g6_1 = new Vector3(-4.98f, 1.4f, 4.41f);
    public static Vector3 g6_2 = new Vector3(-3f, 1.4f, 4.33f);
    public static Vector3 g6_3 = new Vector3(-1.06f, 1.4f, 4.15f);
    public static Vector3 g6_4 = new Vector3(0.92f, 1.4f, 4.010f);
    public static Vector3 g6_5 = new Vector3(2.83f, 1.4f, 3.84f);
    public static Vector3 g6_6 = new Vector3(4.73f, 1.4f, 3.84f);

    // Gato
    // Rotacion x y z
    public static Quaternion GRot = Quaternion.Euler(-90f, 90f, -86.057f);
    // Pocicion x y z
    public static Vector3 G1_1 = new Vector3(-4.39f, 2.2f, 14.11f);
    public static Vector3 G1_2 = new Vector3(-2.49f, 2.2f, 14.03f);
    public static Vector3 G1_3 = new Vector3(-0.51f, 2.2f, 13.86f);
    public static Vector3 G1_4 = new Vector3(1.45f, 2.2f, 13.68f);
    public static Vector3 G1_5 = new Vector3(3.42f, 2.2f, 13.63f);
    public static Vector3 G1_6 = new Vector3(5.3f, 2.2f, 13.49f);
    public static Vector3 G2_1 = new Vector3(-4.57f, 2.2f, 12.2f);
    public static Vector3 G2_2 = new Vector3(-2.63f, 2.2f, 12.1f);
    public static Vector3 G2_3 = new Vector3(-0.66f, 2.2f, 11.98f);
    public static Vector3 G2_4 = new Vector3(1.3f, 2.2f, 11.84f);
    public static Vector3 G2_5 = new Vector3(3.23f, 2.2f, 11.67f);
    public static Vector3 G2_6 = new Vector3(5.16f, 2.2f, 11.5f);
    public static Vector3 G3_1 = new Vector3(-4.7f, 2.2f, 10.25f);
    public static Vector3 G3_2 = new Vector3(-2.76f, 2.2f, 10.02f);
    public static Vector3 G3_3 = new Vector3(-0.79f, 2.2f, 9.93f);
    public static Vector3 G3_4 = new Vector3(1.14f, 2.2f, 9.85f);
    public static Vector3 G3_5 = new Vector3(3.16f, 2.2f, 9.7f);
    public static Vector3 G3_6 = new Vector3(5.03f, 2.2f, 9.53f);
    public static Vector3 G4_1 = new Vector3(-4.8f, 2.2f, 8.12f);
    public static Vector3 G4_2 = new Vector3(-2.92f, 2.2f, 7.99f);
    public static Vector3 G4_3 = new Vector3(-0.94f, 2.2f, 7.87f);
    public static Vector3 G4_4 = new Vector3(1.07f, 2.2f, 7.77f);
    public static Vector3 G4_5 = new Vector3(3f, 2.2f, 7.68f);
    public static Vector3 G4_6 = new Vector3(4.89f, 2.2f, 7.58f);
    public static Vector3 G5_1 = new Vector3(-4.88f, 2.2f, 6.2f);
    public static Vector3 G5_2 = new Vector3(-2.98f, 2.2f, 6.07f);
    public static Vector3 G5_3 = new Vector3(-1.04f, 2.2f, 5.88f);
    public static Vector3 G5_4 = new Vector3(0.89f, 2.2f, 5.74f);
    public static Vector3 G5_5 = new Vector3(2.86f, 2.2f, 5.56f);
    public static Vector3 G5_6 = new Vector3(4.7f, 2.2f, 5.43f);
    public static Vector3 G6_1 = new Vector3(-5.1f, 2.2f, 4.23f);
    public static Vector3 G6_2 = new Vector3(-3.13f, 2.2f, 4.08f);
    public static Vector3 G6_3 = new Vector3(-1.16f, 2.2f, 3.96f);
    public static Vector3 G6_4 = new Vector3(0.76f, 2.2f, 3.78f);
    public static Vector3 G6_5 = new Vector3(2.73f, 2.2f, 3.66f);
    public static Vector3 G6_6 = new Vector3(4.6f, 2.2f, 3.57f);
}