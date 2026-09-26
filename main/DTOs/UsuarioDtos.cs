namespace main.DTOs
{
    // Lo que manda el front en login.js
    public class LoginRequest
    {
        public string dni { get; set; } = null!;
        public string nombre { get; set; } = null!;
        public string apellido { get; set; } = null!;
        public string barrio { get; set; } = null!;
        public string password { get; set; } = null!;
    }

    // Lo que el back le devuelve al front para guardar como "sesión"
    public class SesionResponse
    {
        public int usuarioId { get; set; }
        public int ciudadanoId { get; set; }
        public string dni { get; set; } = null!;
        public string nombre { get; set; } = null!;
        public string apellido { get; set; } = null!;
        public string barrio { get; set; } = null!;
    }
}
