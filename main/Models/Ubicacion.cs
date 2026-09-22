using System;
using System.Collections.Generic;
using System.Text;

namespace main.Models
{
    public class Ubicacion
    {
        // ID de ubicacion de
        [Key]
        public int Id_ubicacion { get; set; }

        // FK -> Reporte
        public int Id_reporte { get; set; }
        
        // Holaaa
        [ForeignKey("id_reporte")]
        public Reporte Reporte { get; set; } = null!;

        // Datos 
        public string calle { get; set; } = null!;
        public int altura { get; set; }
        public int latitud { get; set; }
        public string informacion_adicional { get; set; } = null!;


        public Ubicacion ()
        {

        }
    }
}
