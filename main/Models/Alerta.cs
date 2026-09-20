using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main.Models
{
    internal class Alerta
    {
        // ID
        [Key]
        public int id_alerta {  get; set; }

        // Datos
        public string mensaje { get; set; }
        public DateTime fecha_horario { get; set; }
        public int nivel_urgencia { get; set; }

        // FK -> Tabla conectora
        public ICollection<Barrio_Alerta> Barrios { get; set; } = new List<Barrio_Alerta> ();
    }
}
