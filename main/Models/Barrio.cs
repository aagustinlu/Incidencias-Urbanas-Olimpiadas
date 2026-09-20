using System;
using System.Collections.Generic;
using System.Text;

namespace main.Models
{
    internal class Barrio
    {
        // ID
        [Key]
        public int id_barrio {  get; set; }

        // Relacion N:N Alerta
        public ICollection<Barrio_Alerta> Alertas { get; set; } = new List <Barrio_Alerta> ();

        // Relacion 1:N Ciudadano
        public ICollection<Ciudadano> Ciudadanos { get; set; } = new List <Ciudadano> ();
        
        // Datos
        public string nombre { get; set; }
    }
}