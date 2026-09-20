using System;
using System.Collections.Generic;
using System.Text;

namespace main.Models
{
    internal class Ciudadano
    {
        // ID
        [Key]
        public int id_ciudadano {  get; set; }

        // FK -> Barrio
        public int id_barrio { get; set; }

        [ForeignKey("id_barrio")]
        public Barrio Barrio { get; set; }

        // Propiedad de Navegacion a Usuario
        public Usuario Usuario { get; set; }

        // Datos
        public string nombre { get; set; }
        public string apelldido { get; set; }

    }
}
