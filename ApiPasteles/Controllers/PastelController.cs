using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPasteles.Models;
using ApiPasteles.Models.DTOs;
using Microsoft.AspNetCore.Authorization;


namespace ApiPasteles.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]


    public class PastelController : ControllerBase
    {
        private readonly PastelesContext _Db;
        public PastelController(PastelesContext Db)
        {
            _Db = Db;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var pasteles = await _Db.Pastels
                .Include(p => p.Calificacions)
                .Select(p => new PastelDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Origen = p.Origen,
                    Precio = p.Precio ?? 0,
                    PromedioSabor = p.Calificacions.Any() ? Convert.ToDouble(p.Calificacions.Average(c => c.Sabor)) : 0,
                    PromedioPresentacion = p.Calificacions.Any() ? Convert.ToDouble(p.Calificacions.Average(c => c.Presentacion)) : 0,
                    PromedioFinal = p.Calificacions.Any() ? (Convert.ToDouble(p.Calificacions.Average(c => c.Sabor)) + Convert.ToDouble(p.Calificacions.Average(c => c.Presentacion))) / 2 : 0
                })
                .ToListAsync();

            return StatusCode(StatusCodes.Status200OK, new { value = pasteles });
        }

        [HttpGet]
        [Route("Obtener")]
        public async Task<ActionResult<PastelDTO>> GetPastel(int id)
        {
            var pastel = await _Db.Pastels.FindAsync(id);

            if (pastel == null)
            {
                return NotFound();
            }

            return Ok(new PastelDTO
            {
                Id = pastel.Id,
                Nombre = pastel.Nombre,
                Origen = pastel.Origen,
                Precio = pastel.Precio ?? 0
            });
        }






        [HttpPost]
        [Route("Agregar")]
        public async Task<IActionResult> Agregar(PastelDTO pastl)
        {
            var pastel = new Pastel
            {
                Nombre = pastl.Nombre,
                Origen = pastl.Origen,
                Precio = pastl.Precio
            };

            await _Db.Pastels.AddAsync(pastel);
            await _Db.SaveChangesAsync();

            if (pastel.Id != 0)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
        }


        [HttpPost]
        [Route("Editar")]
        public async Task<IActionResult> Editar(Pastel pastl)
        {
            var pastel = await _Db.Pastels.FindAsync(pastl.Id);
            if (pastel == null)
            {
                return NotFound(new { isSuccess = false, message = "Pastel no encontrado." });
            }

            pastel.Nombre = pastl.Nombre;
            pastel.Origen = pastl.Origen;
            pastel.Precio = pastl.Precio;

            _Db.Pastels.Update(pastel);
            await _Db.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
        }


        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var pastel = await _Db.Pastels.FindAsync(id);
            if (pastel == null)
            {
                return NotFound(new { isSuccess = false, message = "Pastel no encontrado." });
            }

            _Db.Pastels.Remove(pastel);
            await _Db.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
        }

    }
}
