using System;
using System.Collections.Generic;
using System.Text;

namespace main.Models
{
    internal class Usuario
    {
        // ID
        [Key]
        public int id_usuario { get; set; }

        // FK -> Ciudadano 1:1
        public int id_ciudadano { get; set; }

        [ForeignKey("id_ciudadano")]
        public Ciudadano Ciudadano { get; set; }
        
        // Datos
        public string username { get; set; }
        
        public string email { get; set; }
        public string constrasenia { get; set; }
        
        public bool activo { get; set; }
        public bool es_admin {  get; set; }

    }
}
