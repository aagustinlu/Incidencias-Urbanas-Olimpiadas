using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using main.DTOs;
using main.Models;

namespace main.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public UsuariosController(ApplicationDbContext db)
        {
            _db = db;
        }

        // POST /api/usuarios/login
        // Reproduce la lógica que antes vivía en login.js contra localStorage:
        // - Si el DNI no existe todavía, crea Barrio (si hace falta), Ciudadano y Usuario.
        // - Si el DNI ya existe, valida la contraseña.
        [HttpPost("login")]
        public async Task<ActionResult<SesionResponse>> Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.dni) || string.IsNullOrWhiteSpace(req.nombre) ||
                string.IsNullOrWhiteSpace(req.apellido) || string.IsNullOrWhiteSpace(req.barrio) ||
                string.IsNullOrWhiteSpace(req.password))
            {
                return BadRequest(new { mensaje = "Faltan datos obligatorios." });
            }

            var ciudadano = await _db.Ciudadanos
                .Include(c => c.Usuario)
                .Include(c => c.Barrio)
                .FirstOrDefaultAsync(c => c.dni == req.dni);

            if (ciudadano != null)
            {
                // Usuario existente: valida contraseña.
                // NOTA: en este prototipo la contraseña se guarda en texto plano,
                // igual que hacía la versión anterior en localStorage. Para producción
                // habría que hashearla (ej. con BCrypt.Net) antes de comparar/guardar.
                if (ciudadano.Usuario == null || ciudadano.Usuario.constrasenia != req.password)
                {
                    return Unauthorized(new { mensaje = "Contraseña incorrecta." });
                }

                // Si en su momento no tenía barrio cargado, lo completa.
                if (ciudadano.Barrio == null || string.IsNullOrWhiteSpace(ciudadano.Barrio.nombre))
                {
                    var barrioExistente = await GetOrCreateBarrio(req.barrio);
                    ciudadano.Id_barrio = barrioExistente.Id_barrio;
                    await _db.SaveChangesAsync();
                }

                return Ok(new SesionResponse
                {
                    usuarioId = ciudadano.Usuario.Id_usuario,
                    ciudadanoId = ciudadano.Id_ciudadano,
                    dni = ciudadano.dni,
                    nombre = ciudadano.nombre,
                    apellido = ciudadano.apellido,
                    barrio = ciudadano.Barrio?.nombre ?? req.barrio
                });
            }

            // Usuario nuevo: se crea la cuenta con estos datos (igual que el aviso del login.html).
            var barrio = await GetOrCreateBarrio(req.barrio);

            var nuevoCiudadano = new Ciudadano
            {
                dni = req.dni,
                nombre = req.nombre,
                apellido = req.apellido,
                Id_barrio = barrio.Id_barrio
            };
            _db.Ciudadanos.Add(nuevoCiudadano);
            await _db.SaveChangesAsync();

            var nuevoUsuario = new Usuario
            {
                Id_ciudadano = nuevoCiudadano.Id_ciudadano,
                username = req.dni,
                constrasenia = req.password,
                activo = true,
                es_admin = false
            };
            _db.Usuarios.Add(nuevoUsuario);
            await _db.SaveChangesAsync();

            return Ok(new SesionResponse
            {
                usuarioId = nuevoUsuario.Id_usuario,
                ciudadanoId = nuevoCiudadano.Id_ciudadano,
                dni = nuevoCiudadano.dni,
                nombre = nuevoCiudadano.nombre,
                apellido = nuevoCiudadano.apellido,
                barrio = barrio.nombre
            });
        }

        private async Task<Barrio> GetOrCreateBarrio(string nombre)
        {
            var barrio = await _db.Barrios.FirstOrDefaultAsync(b => b.nombre == nombre);
            if (barrio != null) return barrio;

            barrio = new Barrio { nombre = nombre };
            _db.Barrios.Add(barrio);
            await _db.SaveChangesAsync();
            return barrio;
        }
    }
}
