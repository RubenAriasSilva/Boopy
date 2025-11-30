import cv2
import numpy as np
import json
import os

# --- 1. CLASE PARA LA SELECCIÓN DE REGIÓN (Mejorada y Unificada) ---

class SeleccionadorRegion:
    """
    Clase para manejar la selección de una región (línea o polígono) en una imagen.
    Permite guardar y cargar selecciones para reutilizarlas.
    """
    def __init__(self, imagen_referencia_path, modo='poligono'):
        """
        Inicializa el selector.
        :param imagen_referencia_path: Ruta a la imagen que se usará para seleccionar.
        :param modo: 'linea' para seleccionar 2 puntos, 'poligono' para un polígono.
        """
        self.imagen_original = cv2.imread(imagen_referencia_path)
        if self.imagen_original is None:
            raise FileNotFoundError(f"No se pudo cargar la imagen de referencia: {imagen_referencia_path}")
        
        self.imagen_actual = self.imagen_original.copy()
        self.puntos = []
        self.modo = modo
        self.nombre_ventana = f'Selección de Región - Modo: {modo.capitalize()}'
        self.seleccion_confirmada = False

    def mouse_callback(self, event, x, y, flags, param):
        """Maneja los eventos del ratón."""
        if event == cv2.EVENT_LBUTTONDOWN:
            self.puntos.append((x, y))
            print(f"Punto {len(self.puntos)} añadido: ({x}, {y})")
            self._dibujar_seleccion()

    def _dibujar_seleccion(self):
        """Redibuja la imagen con los puntos y las líneas de selección."""
        self.imagen_actual = self.imagen_original.copy()
        if not self.puntos:
            return

        # Dibujar todos los puntos
        for p in self.puntos:
            cv2.circle(self.imagen_actual, p, 5, (0, 255, 0), -1)

        # Dibujar las líneas
        if self.modo == 'linea' and len(self.puntos) == 2:
            cv2.line(self.imagen_actual, self.puntos[0], self.puntos[1], (0, 255, 0), 2)
        elif self.modo == 'poligono' and len(self.puntos) > 1:
            # Dibuja líneas entre puntos consecutivos
            for i in range(len(self.puntos) - 1):
                cv2.line(self.imagen_actual, self.puntos[i], self.puntos[i+1], (0, 255, 0), 2)
            # Cierra el polígono si hay 3 o más puntos
            if len(self.puntos) >= 3:
                cv2.line(self.imagen_actual, self.puntos[-1], self.puntos[0], (0, 255, 0), 2)

    def seleccionar(self):
        """Inicia la ventana de selección y maneja las entradas de teclado."""
        cv2.namedWindow(self.nombre_ventana)
        cv2.setMouseCallback(self.nombre_ventana, self.mouse_callback)

        print(f"\n--- Modo de Selección: {self.modo.upper()} ---")
        print("Instrucciones:")
        print("- Haz clic izquierdo para añadir puntos.")
        if self.modo == 'linea':
            print("- Selecciona 2 puntos para definir una línea.")
        else:
            print("- Selecciona al menos 3 puntos para definir un polígono.")
        print("- Presiona 'ENTER' para confirmar la selección.")
        print("- Presiona 'R' para resetear los puntos.")
        print("- Presiona 'ESC' para cancelar y salir.")

        while True:
            cv2.imshow(self.nombre_ventana, self.imagen_actual)
            key = cv2.waitKey(1) & 0xFF

            if key == 13:  # Tecla ENTER
                if self._validar_seleccion():
                    self.seleccion_confirmada = True
                    print("\n¡Selección confirmada!")
                    break
            elif key == ord('r'):  # Tecla R
                self.puntos = []
                self.imagen_actual = self.imagen_original.copy()
                print("Puntos reseteados.")
            elif key == 27:  # Tecla ESC
                print("\nSelección cancelada.")
                self.puntos = []
                break
        
        cv2.destroyAllWindows()
        return self.puntos if self.seleccion_confirmada else None

    def _validar_seleccion(self):
        """Verifica si la selección actual es válida."""
        if self.modo == 'linea' and len(self.puntos) != 2:
            print("Error: Para el modo 'linea', debes seleccionar exactamente 2 puntos.")
            return False
        if self.modo == 'poligono' and len(self.puntos) < 3:
            print("Error: Para el modo 'poligono', debes seleccionar al menos 3 puntos.")
            return False
        return True

# --- 2. FUNCIONES PARA PERSISTENCIA (Guardar y Cargar Selección) ---

def guardar_seleccion(puntos, modo, filepath):
    """Guarda los puntos de la selección y el modo en un archivo JSON."""
    if not puntos:
        print("No hay puntos para guardar.")
        return False
    try:
        with open(filepath, 'w') as f:
            json.dump({'modo': modo, 'puntos': puntos}, f)
        print(f"Selección guardada exitosamente en '{filepath}'")
        return True
    except IOError as e:
        print(f"Error al guardar la selección: {e}")
        return False

def cargar_seleccion(filepath):
    """Carga los puntos de la selección y el modo desde un archivo JSON."""
    try:
        with open(filepath, 'r') as f:
            data = json.load(f)
        print(f"Selección cargada exitosamente desde '{filepath}'")
        return data['puntos'], data['modo']
    except (IOError, json.JSONDecodeError, KeyError) as e:
        print(f"Error al cargar la selección: {e}")
        return None, None

# --- 3. LÓGICA DE APLICACIÓN DE LA SELECCIÓN ---

def crear_mascara_desde_puntos(shape, puntos, modo):
    """Crea una máscara binaria a partir de una lista de puntos y un modo."""
    h, w = shape[:2]
    mascara = np.zeros((h, w), dtype=np.uint8)

    if modo == 'linea' and len(puntos) == 2:
        p1, p2 = puntos
        # Usamos el producto cruzado para determinar el lado de la línea
        Y, X = np.ogrid[:h, :w]
        dx = p2[0] - p1[0]
        dy = p2[1] - p1[1]
        # Evitar división por cero si la línea es vertical u horizontal
        if dx == 0 and dy == 0: return mascara
        productos_cruz = (X - p1[0]) * dy - (Y - p1[1]) * dx
        mascara = (productos_cruz >= 0).astype(np.uint8) * 255
    elif modo == 'poligono' and len(puntos) >= 3:
        pts = np.array(puntos, np.int32)
        cv2.fillPoly(mascara, [pts], 255)
    
    return mascara

def aplicar_seleccion_a_par(img1_path, img2_path, puntos_seleccion, modo_seleccion):
    """
    Aplica una selección predefinida a un par de imágenes y devuelve el resultado LIMPIO.
    """
    img1 = cv2.imread(img1_path)
    img2 = cv2.imread(img2_path)

    if img1 is None or img2 is None:
        print(f"Error: No se pudieron cargar las imágenes {img1_path} o {img2_path}")
        return None

    if img1.shape != img2.shape:
        print(f"Error: Las imágenes {img1_path} y {img2_path} deben tener el mismo tamaño.")
        return None

    # Crear la máscara una sola vez con los puntos seleccionados
    mascara = crear_mascara_desde_puntos(img1.shape, puntos_seleccion, modo_seleccion)
    
    # Combinar imágenes usando la máscara
    # np.where(condición, valor_si_true, valor_si_false)
    resultado = np.where(mascara[:,:,np.newaxis] == 255, img1, img2)

    # --- CÓDIGO ELIMINADO ---
    # Las siguientes líneas han sido eliminadas para que la imagen final
    # no muestre los puntos ni las líneas de la selección.
    #
    # if modo_seleccion == 'linea':
    #     cv2.line(resultado, puntos_seleccion[0], puntos_seleccion[1], (0, 255, 0), 2)
    # else: # poligono
    #     pts = np.array(puntos_seleccion, np.int32)
    #     cv2.polylines(resultado, [pts], True, (0, 255, 0), 2)
    # 
    # for p in puntos_seleccion:
    #     cv2.circle(resultado, p, 4, (0, 0, 255), -1)

    return resultado

# --- 4. FUNCIÓN PRINCIPAL CON MENÚ ---

def main():
    """
    Función principal que guía al usuario a través del proceso de selección
    y aplicación por lote.
    """
    # --- CONFIGURACIÓN DE IMÁGENES ---
    # Define aquí todos los pares de imágenes que quieres procesar
    # Formato: [(imagen_a_path, imagen_b_path, ruta_salida), ...]
    pares_imagenes = [
        ('GatoPrefab1_icon.png', 'GatoPrefab2_icon.png', 'GatoCosmetico1.png'),
        ('GatoPrefab3_icon.png', 'GatoPrefab4_icon.png', 'GatoCosmetico2.png'),
        # Añade más pares aquí si es necesario
        # ('imagen5_a.png', 'imagen5_b.png', 'resultado5.png'),
    ]
    
    # --- MENÚ PRINCIPAL ---
    print("=== COMBINADOR DE IMÁGENES POR LOTE ===")
    print("¿Qué quieres hacer?")
    print("1. Crear una nueva selección y aplicarla.")
    print("2. Cargar una selección existente y aplicarla.")
    print("3. Salir.")

    opcion = input("Selecciona una opción (1, 2 o 3): ")

    puntos_seleccion = None
    modo_seleccion = None

    if opcion == '1':
        # --- Opción 1: Crear nueva selección ---
        print("\n--- Creando Nueva Selección ---")
        modo = input("Elige el modo de selección ('linea' o 'poligono'): ").lower()
        if modo not in ['linea', 'poligono']:
            print("Modo no válido. Saliendo.")
            return

        # Usamos la primera imagen del primer par como referencia
        if not pares_imagenes:
            print("No hay pares de imágenes configurados. Saliendo.")
            return
            
        imagen_referencia_path = pares_imagenes[0][0]
        print(f"\nUsando '{imagen_referencia_path}' como imagen de referencia.")
        print("Abriendo ventana de selección...")

        selector = SeleccionadorRegion(imagen_referencia_path, modo=modo)
        puntos_seleccion = selector.seleccionar()

        if puntos_seleccion:
            modo_seleccion = modo
            guardar = input("¿Quieres guardar esta selección para usarla después? (s/n): ").lower()
            if guardar == 's':
                ruta_guardado = "mi_seleccion.json"
                guardar_seleccion(puntos_seleccion, modo_seleccion, ruta_guardado)

    elif opcion == '2':
        # --- Opción 2: Cargar selección existente ---
        print("\n--- Cargando Selección Existente ---")
        ruta_carga = input("Introduce la ruta del archivo de selección (.json): ")
        puntos_seleccion, modo_seleccion = cargar_seleccion(ruta_carga)

    else:
        print("Opción no válida o saliendo del programa.")
        return

    # --- FASE DE APLICACIÓN POR LOTE ---
    if puntos_seleccion and modo_seleccion:
        print("\n--- Iniciando Procesamiento por Lote ---")
        for i, (img_a, img_b, salida) in enumerate(pares_imagenes):
            print(f"\nProcesando par {i+1}/{len(pares_imagenes)}: '{img_a}' + '{img_b}' -> '{salida}'")
            resultado = aplicar_seleccion_a_par(img_a, img_b, puntos_seleccion, modo_seleccion)
            
            if resultado is not None:
                cv2.imwrite(salida, resultado)
                print(f"¡Éxito! Imagen guardada en: {salida}")
                
                # Opcional: mostrar la imagen resultante
                # cv2.imshow(f'Resultado {i+1}', resultado)
                # cv2.waitKey(500) # Muestra la imagen por 500ms

        print("\n¡Procesamiento de todos los pares finalizado!")
        # cv2.destroyAllWindows() # Descomentar si se usó el display opcional

if __name__ == "__main__":
    main()