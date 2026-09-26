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

        // FK -> Usuario (quien hizo el reporte)
        public int Id_usuario { get; set; }

        [ForeignKey("Id_usuario")]
        public Usuario Usuario { get; set; } = null!;

        // 1:1 con Ubicacion. La FK vive del lado de Ubicacion (Ubicacion.Id_reporte).
        public Ubicacion? Ubicacion { get; set; }

        // Datos
        public string categoria { get; set; } = null!;
        public string titulo { get; set; } = null!;
        public string? descripcion { get; set; }
        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public DateTime fecha_modificacion { get; set; } = DateTime.Now;
        public bool activo { get; set; } = true;


        public Reporte ()
        {

        }
    }
}
