using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPasteles.Custom;
using ApiPasteles.Models;
using ApiPasteles.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ApiPasteles.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]  
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly PastelesContext _Db;
        private readonly Utilidades _utilidades;
        public AccesoController(PastelesContext Db, Utilidades utilidades)
        {
            _Db = Db;
            _utilidades = utilidades;
        }

        [HttpPost]
        [Route("Registarse")]
        public async Task<IActionResult> Registrarse(UsuarioDTO objet)
        {
            var modelUser = new Usuario
            {
                Nombre = objet.Nombre,
                Email = objet.Email,
                Clave = _utilidades.encriptarSHA256(objet.clave)
            };

            await _Db.Usuarios.AddAsync(modelUser);
            await _Db.SaveChangesAsync();

            if (modelUser.Id != 0)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO objet)
        {
            var UserFound = await _Db.Usuarios.Where(u => u.Email == objet.Email &&
                                                u.Clave == _utilidades.encriptarSHA256(objet.clave))
                                                .FirstOrDefaultAsync();

            if (UserFound == null)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token ="", userId = "", userName = "" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidades.generarJwt(UserFound),
                    userId = UserFound.Id.ToString(),
                    userName = UserFound.Nombre
                });

        }


    }
}
