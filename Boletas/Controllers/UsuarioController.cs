using Boletas.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Boletas.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioController(IUsuarioRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                var result = await _repo.GetUsuarios();
                return Ok(new
                {
                    data = result,
                    message = string.Empty,
                    status = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Login([FromQuery] string usuario, [FromQuery] string pass)
        {
            try
            {
                var result = await _repo.Login(usuario, pass);
                if (result is null)
                {
                    return Unauthorized(new
                    {
                        data = false,
                        message = "Credenciales invalidas.",
                        status = false
                    });
                }

                return Ok(new
                {
                    data = result,
                    message = string.Empty,
                    status = true
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    data = false,
                    message = ex.Message,
                    status = false
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
