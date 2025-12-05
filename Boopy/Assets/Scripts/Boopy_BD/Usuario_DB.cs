using System;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BoopyGame
{
    // El nombre debe coincidir con tu tabla en Supabase
    [Table("USUARIOS")]
    public class UsuarioModelo : BaseModel
    {
        // 'id' es PK y es autogenerado por la base de datos (false)
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("nickname")]
        public string Nombre { get; set; }

        [Column("correo")]
        public string Correo { get; set; }

        [Column("password")]
        public string Password { get; set; }

        // FK
        [Column("gatito_id")]
        public int GatitoId { get; set; }

        [Column("gatote_id")]
        public int GatoteId { get; set; }

        [Column("idioma")]
        public int IdiomaSeleccionadoId { get; set; }
    }
}