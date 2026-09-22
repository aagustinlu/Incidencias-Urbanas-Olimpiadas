using System;
using System.Collections.Generic;
using System.Text;

namespace main.Models
{
    public class Usuario
    {
        // ID
        [Key]
        public int Id_usuario { get; set; }

        // FK -> Ciudadano 1:1
        public int Id_ciudadano { get; set; }

        [ForeignKey("id_ciudadano")]
        public Ciudadano Ciudadano { get; set; }

        // Relacion Reporte N:N
        public ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();


        // Datos
        public string username { get; set; } = null!;

        // Apuntes:
        // Cuando declaramos un campo de un atributo para la base de datos pueden darse 3 casos

        // A: Queremos que el campo sea opcional en la base de datos (Se admite null). Se hace asi:
        // public string? Telefono {get; set; }

        // B: El campo es obligatorio y lo inicializamos con texto vacio:
        // public string Nombre {get; set} = string.Empty ;

        // C: El campo es obligatorio y queremos suprimir la advertencia:
        // public string Email {get; set;} = null! ;

        public string email { get; set; } = null!;
        public string constrasenia { get; set; } = null!;
        
        public bool activo { get; set; }
        public bool es_admin {  get; set; }


        public Usuario ()
        {

        }
    }
}
