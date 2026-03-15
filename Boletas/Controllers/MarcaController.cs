using Boletas.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Boletas.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MarcaController : ControllerBase
    {
        private readonly IMarcaRepository _repo;

        public MarcaController(IMarcaRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetMarcas()
        {
            try
            {
                var result = await _repo.GetMarcas();
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
