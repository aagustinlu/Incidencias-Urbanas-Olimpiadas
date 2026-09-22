using System;
using System.Collections.Generic;
using System.Text;

namespace main.Models
{
    public class Barrio
    {
        // ID
        [Key]
        public int Id_barrio {  get; set; }

        // Relacion N:N Alerta
        public ICollection<Barrio_Alerta> BarrioAlertas { get; set; } = new List <Barrio_Alerta> ();

        // Relacion 1:N Ciudadano
        public ICollection<Ciudadano> Ciudadanos { get; set; } = new List <Ciudadano> ();
        
        // Datos
        public string nombre { get; set; } = null!;


        public Barrio ()
        {

        }
    }
}