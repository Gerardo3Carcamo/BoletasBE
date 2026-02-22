using Boletas.Data;
using Boletas.DTOs;
using Boletas.Models;
using Boletas.Repositories.Interfaces;

namespace Boletas.Repositories
{
    public class BoletaRepository : IBoletaRepository
    {
        private readonly BoletasDbContext _context;

        public BoletaRepository(BoletasDbContext context) => _context = context;

        public async Task InsertBoleta(BoletaDto payload)
        {
            try
            {
                payload.Boletas.Folio = "test";
                _context.Boletas.Add(payload.Boletas);
                await _context.SaveChangesAsync();
                foreach(Usuario u in payload.Usuarios)
                {
                    _context.BoletaUsuarios.Add(new BoletaUsuario
                    {
                       IdBoleta = payload.Boletas.Id,
                       NombreUsuario = u.Nombre,
                       Nomina = u.Nomina,
                       Direccion = u.Direccion 
                    });
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}