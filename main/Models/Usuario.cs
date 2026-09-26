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

        [ForeignKey("Id_ciudadano")]
        public Ciudadano Ciudadano { get; set; } = null!;

        // Relacion Reporte N:N
        public ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();


        // Datos
        public string username { get; set; } = null!;
        public string email { get; set; } = string.Empty;
        public string constrasenia { get; set; } = null!;

        public bool activo { get; set; } = true;
        public bool es_admin { get; set; }


        public Usuario ()
        {

        }
    }
}
