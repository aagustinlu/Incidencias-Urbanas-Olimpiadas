using System;
using System.Collections.Generic;
using System.Text;

namespace main.Models
{
    internal class Ubicacion
    {
        // ID de ubicacion
        [Key]
        public int id_ubicacion { get; set; }

        // FK -> Reporte
        public int id_reporte { get; set; }
        
        // Holaaa
        [ForeignKey("id_reporte")]
        public Reporte Reporte { get; set; }

        // Datos 
        public string calle { get; set; }
        public int altura { get; set; }
        public int latitud { get; set; }
        public string informacion_adicional { get; set; }
        
    }
}
