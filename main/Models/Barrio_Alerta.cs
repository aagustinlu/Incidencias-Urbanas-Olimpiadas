using System;
using System.Collections.Generic;
using System.Text;

namespace main.Models
{
    public class Barrio_Alerta
    {
        // ID
        [Key]
        public int Id_barrio_alerta {  get; set; }
        
        // FK -> Barrio
        public int Id_barrio { get; set; }

        [ForeignKey("id_barrio")]
        public Barrio Barrio { get; set; } = null!;

        // FK -> Alerta
        public int Id_alerta { get; set; }

        [ForeignKey("id_alerta")]
        public Alerta Alerta { get; set; } = null!;


        public Barrio_Alerta ()
        {
        }
    }
}
