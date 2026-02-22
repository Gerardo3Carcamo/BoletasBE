using System.Linq;
using System.Threading.Tasks;
using Boletas.DTOs;
using Boletas.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Boletas.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class BoletaController: ControllerBase
    {

        private readonly IBoletaRepository _repo;

        public BoletaController(IBoletaRepository repo) => _repo = repo;

        [HttpPost]
        public async Task<IActionResult> AddBoleta(BoletaDto payload)
        {
            try
            {
                await _repo.InsertBoleta(payload);
                return Ok(new
                {
                    data = true,
                    message = "",
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