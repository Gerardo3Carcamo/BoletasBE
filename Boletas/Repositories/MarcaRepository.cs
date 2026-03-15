using Boletas.Data;
using Boletas.Models;
using Boletas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Boletas.Repositories
{
    public class MarcaRepository : IMarcaRepository
    {
        private readonly BoletasDbContext _context;

        public MarcaRepository(BoletasDbContext context) => _context = context;

        public async Task<IReadOnlyList<Marca>> GetMarcas()
        {
            return await _context.Marcas
                .AsNoTracking()
                .OrderBy(m => m.Nombre)
                .ToListAsync();
        }
    }
}
