using ApiPasteles.Models;
using ApiPasteles.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ApiPasteles.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CalificacionController : ControllerBase
    {
        private readonly PastelesContext _Db;
        public CalificacionController(PastelesContext Db)
        {
            _Db = Db;
        }

        [HttpGet]
        [Route("Calificaciones")]
        public async Task<IActionResult> Lista()
        {
            var lista = await _Db.Calificacions
                .Include(c => c.UsuarioNavigation)
                .Include(c => c.PastelNavigation)
                .Select(c => new CalificiacionDTO
                {
                    Id = c.Id,
                    IdUsuario = c.UsuarioNavigation.Id,
                    Usuario = c.UsuarioNavigation.Nombre,
                    IdPastel = c.PastelNavigation.Id,
                    Pastel = c.PastelNavigation.Nombre,
                    Sabor = c.Sabor,
                    Presentacion = c.Presentacion
                })
                .ToListAsync();

            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }

        [HttpGet]
        [Route("Califi")]
        public async Task<IActionResult> Lista(int userId)
        {
            var lista = await _Db.Calificacions
                .Include(c => c.UsuarioNavigation)
                .Include(c => c.PastelNavigation)
                .Where(c => c.UsuarioNavigation.Id == userId) 
                .Select(c => new CalificiacionDTO
                {
                    Id = c.Id,
                    Usuario = c.UsuarioNavigation.Nombre,
                    Pastel = c.PastelNavigation.Nombre,
                    Sabor = c.Sabor,
                    Presentacion = c.Presentacion
                })
                .ToListAsync();

            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }

        [HttpGet]
        [Route("VistaCalificacion")]
        public async Task<IActionResult> MiVista(int id)
        {
            var calificacion = await _Db.Calificacions
                .Include(c => c.UsuarioNavigation)
                .Include(c => c.PastelNavigation)
                .Where(c => c.Id == id)
                .Select(c => new CalificiacionDTO
                {
                    Id = c.Id,
                    Usuario = c.UsuarioNavigation.Nombre,
                    Pastel = c.PastelNavigation.Nombre,
                    IdUsuario = c.Usuario,
                    IdPastel = c.Pastel,
                    Sabor = c.Sabor,
                    Presentacion = c.Presentacion
                })
                .FirstOrDefaultAsync();

            if (calificacion == null)
            {
                return NotFound();
            }

            return Ok(calificacion);
        }



        [HttpPost]
        [Route("Calificar")]
        public async Task<IActionResult> Califiacr( CalificiacionDTO cali)
        {

            var existeCalificacion = await _Db.Calificacions.FirstOrDefaultAsync(c =>
                c.Usuario == cali.IdUsuario && c.Pastel == cali.IdPastel);

            if (existeCalificacion != null)
            {
                return Conflict("El usuario ya ha calificado este pastel anteriormente.");
            }
            var calif = new Calificacion
            {
                Usuario = cali.IdUsuario,
                Pastel = cali.IdPastel,
                Sabor = cali.Sabor,
                Presentacion = cali.Presentacion,
            };

            await _Db.Calificacions.AddAsync(calif);
            await _Db.SaveChangesAsync();

            if (calif.Id != 0)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
        }


        [HttpPost]
        [Route("EditarCali")]
        public async Task<IActionResult> EditarCali(CalificiacionDTO calf)
        {
            
            var cals = await _Db.Calificacions.FindAsync(calf.Id);
            if (cals == null)
            {
                return NotFound(new { isSuccess = false });
            }

            cals.Usuario = calf.IdUsuario;
            cals.Pastel = calf.IdPastel;
            cals.Sabor = calf.Sabor;
            cals.Presentacion = calf.Presentacion;

            _Db.Calificacions.Update(cals);
            await _Db.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var cali = await _Db.Calificacions.FindAsync(id);
            if (cali == null)
            {
                return NotFound(new { isSuccess = false, message = "Calificacion no encontrado." });
            }

            _Db.Calificacions.Remove(cali);
            await _Db.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
        }

    }
}
