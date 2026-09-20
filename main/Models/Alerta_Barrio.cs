using System;
using System.Collections.Generic;
using System.Text;

namespace main.Models
{
    internal class Barrio_Alerta
    {
        // ID
        [Key]
        public int id_barrio_alerta {  get; set; }
        
        // FK -> Barrio
        public int id_barrio { get; set; }

        [ForeignKey("id_barrio")]
        public Barrio Barrio { get; set; }
        
        // FK -> Alerta
        public int id_alerta { get; set; }

        [ForeignKey("id_alerta")]
        public Alerta Alerta { get; set; }
    }
}
