using System;
using System.Collections.Generic;
using System.Text;

namespace main.Models
{
    public class Ciudadano
    {
        // ID
        [Key]
        public int Id_ciudadano {  get; set; }

        // FK -> Barrio
        public int Id_barrio { get; set; }

        [ForeignKey("id_barrio")]
        public Barrio Barrio { get; set; } = null!;

        // Propiedad de Navegacion a Usuario
        public Usuario Usuario { get; set; } = null!;

        // Datos
        public string nombre { get; set; } = null!;
        public string apellido { get; set; } = null!;


        public Ciudadano ()
        { 
        }

    }
}
