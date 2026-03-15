using Boletas.Data;
using Boletas.DTOs;
using Boletas.Models;
using Boletas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Boletas.Repositories
{
    public class BoletaRepository : IBoletaRepository
    {
        private readonly BoletasDbContext _context;

        public BoletaRepository(BoletasDbContext context) => _context = context;

        public async Task<object> GetBoleta(int id)
        {
            var result = await (
                from b in _context.Boletas.AsNoTracking()
                join o in _context.Operadores.AsNoTracking() on b.IdOperador equals o.Id into operadores
                from o in operadores.DefaultIfEmpty()
                join p in _context.Plantas.AsNoTracking() on b.IdPlanta equals p.Id into plantas
                from p in plantas.DefaultIfEmpty()
                join t in _context.Traslados.AsNoTracking() on b.IdTraslado equals t.Id into traslados
                from t in traslados.DefaultIfEmpty()
                join tu in _context.Turnos.AsNoTracking() on b.IdTurno equals tu.Id into turnos
                from tu in turnos.DefaultIfEmpty()
                join u in _context.Unidades.AsNoTracking() on b.IdUnidad equals u.Id into unidades
                from u in unidades.DefaultIfEmpty()
                where b.Id == id
                select new
                {
                    Boletas = new
                    {
                        Id = b.Id,
                        Traslado = t != null ? t.Tipo : string.Empty,
                        Turno = tu != null ? tu.Descripcion : string.Empty,
                        Unidad = u != null ? u.NumeroUnidad : string.Empty,
                        Operador = o != null ? o.Nombre : string.Empty,
                        Planta = p != null ? p.Nombre : string.Empty,
                        Creado = b.TsCarga,
                        Entrada = b.HoraEntrada,
                        Salida = b.HoraSalida,
                        Supervisor = b.NombreSupervisor,
                        SupervisorPlanta = b.NombreSupervisorPlanta,
                        Folio = b.Folio
                    },
                    Usuarios = _context.BoletaUsuarios
                        .AsNoTracking()
                        .Where(x => x.IdBoleta == b.Id)
                        .Select(x => new
                        {
                            id = x.Id,
                            idBoleta = x.IdBoleta,
                            nomina = x.Nomina,
                            nombreUsuario = x.NombreUsuario,
                            direccion = x.Direccion
                        })
                        .ToList()
                }).FirstOrDefaultAsync();

            if (result is null)
            {
                throw new InvalidOperationException("Boleta no encontrada.");
            }

            return result;
        }

        public async Task<IReadOnlyList<object>> GetBoletas()
        {
            return await (
                from b in _context.Boletas.AsNoTracking()
                join o in _context.Operadores.AsNoTracking() on b.IdOperador equals o.Id into operadores
                from o in operadores.DefaultIfEmpty()
                join p in _context.Plantas.AsNoTracking() on b.IdPlanta equals p.Id into plantas
                from p in plantas.DefaultIfEmpty()
                join t in _context.Traslados.AsNoTracking() on b.IdTraslado equals t.Id into traslados
                from t in traslados.DefaultIfEmpty()
                join tu in _context.Turnos.AsNoTracking() on b.IdTurno equals tu.Id into turnos
                from tu in turnos.DefaultIfEmpty()
                join u in _context.Unidades.AsNoTracking() on b.IdUnidad equals u.Id into unidades
                from u in unidades.DefaultIfEmpty()
                orderby b.TsCarga descending
                select (object)new
                {
                    Boletas = new
                    {
                        Id = b.Id,
                        Traslado = t != null ? t.Tipo : string.Empty,
                        Turno = tu != null ? tu.Descripcion : string.Empty,
                        Unidad = u != null ? u.NumeroUnidad : string.Empty,
                        Operador = o != null ? o.Nombre : string.Empty,
                        Planta = p != null ? p.Nombre : string.Empty,
                        Creado = b.TsCarga,
                        Entrada = b.HoraEntrada,
                        Salida = b.HoraSalida,
                        Supervisor = b.NombreSupervisor,
                        SupervisorPlanta = b.NombreSupervisorPlanta,
                        Folio = b.Folio
                    },
                    Usuarios = _context.BoletaUsuarios
                        .AsNoTracking()
                        .Where(x => x.IdBoleta == b.Id)
                        .Select(x => new
                        {
                            id = x.Id,
                            idBoleta = x.IdBoleta,
                            nomina = x.Nomina,
                            nombreUsuario = x.NombreUsuario,
                            direccion = x.Direccion
                        })
                        .ToList()
                }).ToListAsync();
        }

        public async Task<object> InsertBoleta(BoletaDto payload)
        {
            if (payload?.Boletas is null)
            {
                throw new ArgumentException("La boleta es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(payload.Boletas.NombreSupervisor))
            {
                throw new ArgumentException("El nombre del supervisor es obligatorio.");
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            payload.Boletas.TsCarga = payload.Boletas.TsCarga == default ? DateTime.UtcNow : payload.Boletas.TsCarga;
            payload.Boletas.Folio = string.Empty;

            _context.Boletas.Add(payload.Boletas);
            await _context.SaveChangesAsync();

            payload.Boletas.Folio = BuildFolio(payload.Boletas);
            await _context.SaveChangesAsync();

            if (payload.Usuarios is not null && payload.Usuarios.Count > 0)
            {
                foreach (var usuario in payload.Usuarios.Where(u =>
                             !string.IsNullOrWhiteSpace(u.Nombre) &&
                             !string.IsNullOrWhiteSpace(u.Nomina)))
                {
                    _context.BoletaUsuarios.Add(new BoletaUsuario
                    {
                        IdBoleta = payload.Boletas.Id,
                        NombreUsuario = usuario.Nombre.Trim(),
                        Nomina = usuario.Nomina.Trim(),
                        Direccion = usuario.Direccion?.Trim() ?? string.Empty
                    });
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new
            {
                id = payload.Boletas.Id,
                folio = payload.Boletas.Folio
            };
        }

        private static string BuildFolio(Boleta boleta)
        {
            var supervisor = boleta.NombreSupervisor.Trim();
            var inicial = supervisor[0].ToString().ToUpperInvariant();
            return $"{inicial}{boleta.IdOperador}{boleta.IdPlanta}{boleta.IdTraslado}{boleta.IdUnidad}{boleta.Id}";
        }
    }
}
