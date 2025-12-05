using System.Linq;
using UnityEngine;
using Supabase;
using System.Threading.Tasks;
using System;

namespace BoopyGame
{
    public class GameManager_DB : MonoBehaviour
    {
        // Singleton: Para poder llamarlo desde cualquier lado con GameManager_DB.Instance
        public static GameManager_DB Instance;
        
        public UsuarioModelo usuarioActual { get; set; }

        public Supabase.Client client;

        // TUS CREDENCIALES
        private string url = "https://pmoiabbtfduqqvreqimz.supabase.co";
        private string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InBtb2lhYmJ0ZmR1cXF2cmVxaW16Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjQ4NjM5OTUsImV4cCI6MjA4MDQzOTk5NX0.v4kds2sILc76aW8devXAYG3XjaGGujJAswdXrliv1ok";

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // ¡Que sobreviva entre escenas!
                InicializarSupabase();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private async void InicializarSupabase()
        {
            var options = new Supabase.SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            client = new Supabase.Client(url, key, options);
            await client.InitializeAsync();
            Debug.Log("Supabase conectado y listo.");
        }

        // --- AQUÍ VA TU FUNCIÓN ---
        public async Task<bool> CrearUsuario(string nombre, string correo, string password)
        {
            try
            {
                var nuevoUsuario = new UsuarioModelo
                {
                    Nombre = nombre,
                    Correo = correo,
                    Password = password,                    
                    
                    GatitoId = 1, 
                    GatoteId = 1,
                    IdiomaSeleccionadoId = 1
                };

                await client.From<UsuarioModelo>().Insert(nuevoUsuario);
                
                Debug.Log("Usuario creado exitosamente en la nube.");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error al crear usuario: {e.Message}");
                return false;
            }
        }

        public async Task<bool> ValidarLogin(string correoInput, string passwordInput)
        {
            try
            {                
                var respuesta = await client.From<UsuarioModelo>()
                    .Select("*")
                    .Where(u => u.Correo == correoInput && u.Password == passwordInput)
                    .Get();

                var usuarioEncontrado = respuesta.Model;

                if (usuarioEncontrado != null)
                {
                    Debug.Log($"¡Login Exitoso! Bienvenido {usuarioEncontrado.Nombre} (ID: {usuarioEncontrado.Id})");

                    usuarioActual = usuarioEncontrado;
                    
                    return true;
                }
                else
                {
                    Debug.Log("Correo o contraseña incorrectos.");
                    return false;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error en el Login: " + e.Message);
                return false;
            }
        }

    }
}