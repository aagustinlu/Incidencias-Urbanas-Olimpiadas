using System;
using System.Collections.Generic;
using System.Text;

namespace main.Models
{
    internal class Reporte
    {
        // ID
        public int id_reporte {  get; set; }

        public int id_ubicacion { get; set; }

        // FK -> Usuario
        public int id_usuario { get; set; }

        [ForeignKey("id_usuario")]
        public Usuario Usuario { get; set; }

        // Datos
        public DateTime fecha_creacion { get; set; }
        public string descripcion { get; set; }
        public DateTime fecha_modificacion { get; set; }
        public bool activo { get; set; }
    }
}
