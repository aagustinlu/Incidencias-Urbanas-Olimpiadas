namespace main.DTOs
{
    // Lo que manda el front al crear un reporte (formIncidencia en script.js)
    public class ReporteCreateRequest
    {
        public string categoria { get; set; } = null!;
        public string titulo { get; set; } = null!;
        public string direccion { get; set; } = null!;
        public string? detalle { get; set; }
        public int usuarioId { get; set; }
    }

    // Lo que el back le devuelve al front para listar/renderizar (render() en script.js)
    public class ReporteResponse
    {
        public int id { get; set; }
        public string categoria { get; set; } = null!;
        public string titulo { get; set; } = null!;
        public string direccion { get; set; } = null!;
        public string? detalle { get; set; }
        public string fecha { get; set; } = null!;
    }
}
