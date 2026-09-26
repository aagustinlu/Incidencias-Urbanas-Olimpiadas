using System;
using System.Collections.Generic;
using System.Text;

namespace main.Models
{
    public class Ubicacion
    {
        // ID de ubicacion
        [Key]
        public int Id_ubicacion { get; set; }

        // FK -> Reporte (1:1). Este es el lado que guarda la clave foránea.
        public int Id_reporte { get; set; }

        [ForeignKey("Id_reporte")]
        public Reporte Reporte { get; set; } = null!;

        // Datos 
        public string calle { get; set; } = null!;
        public int altura { get; set; }
        public int latitud { get; set; }
        public string? informacion_adicional { get; set; }


        public Ubicacion ()
        {

        }
    }
}
