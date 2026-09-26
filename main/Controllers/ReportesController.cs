using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using main.DTOs;
using main.Models;

namespace main.Controllers
{
    [ApiController]
    [Route("api/reportes")]
    public class ReportesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ReportesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET /api/reportes  -> lista para pintar en "Reportes recientes" (render() en script.js)
        [HttpGet]
        public async Task<ActionResult<List<ReporteResponse>>> GetAll()
        {
            var reportes = await _db.Reportes
                .Include(r => r.Ubicacion)
                .Where(r => r.activo)
                .OrderByDescending(r => r.fecha_creacion)
                .Select(r => new ReporteResponse
                {
                    id = r.Id_reporte,
                    categoria = r.categoria,
                    titulo = r.titulo,
                    direccion = r.Ubicacion != null ? r.Ubicacion.calle : "",
                    detalle = r.descripcion,
                    fecha = r.fecha_creacion.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            return Ok(reportes);
        }

        // POST /api/reportes  -> crea un reporte + su ubicación (formIncidencia en script.js)
        [HttpPost]
        public async Task<ActionResult<ReporteResponse>> Create([FromBody] ReporteCreateRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.categoria) || string.IsNullOrWhiteSpace(req.titulo) ||
                string.IsNullOrWhiteSpace(req.direccion))
            {
                return BadRequest(new { mensaje = "Faltan datos obligatorios." });
            }

            var usuarioExiste = await _db.Usuarios.AnyAsync(u => u.Id_usuario == req.usuarioId);
            if (!usuarioExiste)
            {
                return BadRequest(new { mensaje = "El usuario de la sesión no es válido. Volvé a iniciar sesión." });
            }

            var reporte = new Reporte
            {
                Id_usuario = req.usuarioId,
                categoria = req.categoria,
                titulo = req.titulo,
                descripcion = req.detalle,
                fecha_creacion = DateTime.Now,
                fecha_modificacion = DateTime.Now,
                activo = true
            };
            _db.Reportes.Add(reporte);
            await _db.SaveChangesAsync();

            var ubicacion = new Ubicacion
            {
                Id_reporte = reporte.Id_reporte,
                calle = req.direccion,
                altura = 0,
                latitud = 0,
                informacion_adicional = null
            };
            _db.Ubicaciones.Add(ubicacion);
            await _db.SaveChangesAsync();

            return Ok(new ReporteResponse
            {
                id = reporte.Id_reporte,
                categoria = reporte.categoria,
                titulo = reporte.titulo,
                direccion = ubicacion.calle,
                detalle = reporte.descripcion,
                fecha = reporte.fecha_creacion.ToString("dd/MM/yyyy")
            });
        }
    }
}
