using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main.Models
{
    public class Alerta
    {
        // ID
        [Key]
        public int Id_alerta {  get; set; }

        // Datos
        public string mensaje { get; set; } = null!;
        public DateTime fecha_horario { get; set; } = DateTime.Now;
        public int nivel_urgencia { get; set; }

        // FK -> Tabla conectora
        public ICollection<Barrio_Alerta> BarriosAlertas { get; set; } = new List<Barrio_Alerta> ();


        // Constructores

        // Constructor por defecto
        public Alerta ()
        {
        }

        // Constructor parametrizado
        public Alerta (int Id_alerta, string mensaje, DateTime fecha_horario, int nivel_urgencia)
        {
            this.Id_alerta = Id_alerta;
            this.mensaje = mensaje;
            this.fecha_horario = fecha_horario;
            this.nivel_urgencia = nivel_urgencia;
        }
    }
}
