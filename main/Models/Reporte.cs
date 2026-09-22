using System;
using System.Collections.Generic;
using System.Text;

namespace main.Models
{
    public class Reporte
    {
        // ID
        [Key]
        public int Id_reporte {  get; set; }

        // FK -> Ubicacion
        public int Id_ubicacion { get; set; }

        // FK -> Usuario
        public int Id_usuario { get; set; }

        [ForeignKey("id_usuario")]
        public Usuario Usuario { get; set; } = null!;

        // Datos
        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public string? descripcion { get; set; } 
        public DateTime fecha_modificacion { get; set; }
        public bool activo { get; set; }


        public Reporte ()
        {

        }
    }
}
