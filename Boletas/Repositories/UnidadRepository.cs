using Boletas.Data;
using Boletas.DTOs;
using Boletas.Models;
using Boletas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Boletas.Repositories
{
    public class UnidadRepository : IUnidadRepository
    {
        private readonly BoletasDbContext _context;

        public UnidadRepository(BoletasDbContext context) => _context = context;

        public async Task<object> CrearUnidad(CrearUnidadDto payload)
        {
            if (payload.Marca <= 0)
            {
                throw new ArgumentException("La marca es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(payload.Modelo))
            {
                throw new ArgumentException("El modelo es obligatorio.");
            }

            if (payload.Anio <= 0)
            {
                throw new ArgumentException("El anio es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(payload.NumeroUnidad))
            {
                throw new ArgumentException("El numero de unidad es obligatorio.");
            }

            var marca = await _context.Marcas
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == payload.Marca);

            if (marca is null)
            {
                throw new InvalidOperationException("La marca seleccionada no existe.");
            }

            var numeroUnidad = payload.NumeroUnidad.Trim();
            var exists = await _context.Unidades
                .AnyAsync(u => u.NumeroUnidad == numeroUnidad);

            if (exists)
            {
                throw new InvalidOperationException("Ya existe una unidad con ese numero.");
            }

            var unidad = new Unidad
            {
                NumeroUnidad = numeroUnidad,
                Marca = marca.Nombre,
                Modelo = $"{payload.Modelo.Trim()} {payload.Anio}".Trim()
            };

            _context.Unidades.Add(unidad);
            await _context.SaveChangesAsync();

            return new
            {
                id = unidad.Id,
                numeroUnidad = unidad.NumeroUnidad,
                marca = marca.Id,
                modelo = payload.Modelo.Trim(),
                anio = payload.Anio,
                descripcion = unidad.Modelo
            };
        }

        public async Task<IReadOnlyList<object>> GetUnidades()
        {
            return await _context.Unidades
                .AsNoTracking()
                .OrderBy(u => u.NumeroUnidad)
                .Select(u => (object)new
                {
                    id = u.Id,
                    numeroUnidad = u.NumeroUnidad,
                    marca = u.Marca,
                    modelo = u.Modelo
                })
                .ToListAsync();
        }
    }
}
