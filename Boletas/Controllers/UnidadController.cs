using Boletas.DTOs;
using Boletas.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Boletas.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UnidadController : ControllerBase
    {
        private readonly IUnidadRepository _repo;

        public UnidadController(IUnidadRepository repo) => _repo = repo;

        [HttpPost]
        public async Task<IActionResult> CrearUnidad(CrearUnidadDto payload)
        {
            try
            {
                var result = await _repo.CrearUnidad(payload);
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
            catch (InvalidOperationException ex)
            {
                return Conflict(new
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

        [HttpGet]
        public async Task<IActionResult> GetUnidades()
        {
            try
            {
                var result = await _repo.GetUnidades();
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
    }
}
