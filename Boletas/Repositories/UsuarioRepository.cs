using Boletas.Data;
using Boletas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Boletas.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly BoletasDbContext _context;

        public UsuarioRepository(BoletasDbContext context) => _context = context;

        public async Task<IReadOnlyList<object>> GetUsuarios()
        {
            return await _context.UsuariosSistema
                .AsNoTracking()
                .OrderBy(u => u.Usuario)
                .Select(u => (object)new
                {
                    id = u.Id,
                    nombres = u.Nombres,
                    apellidos = u.Apellidos,
                    usuario = u.Usuario,
                    rol = u.Rol
                })
                .ToListAsync();
        }

        public async Task<object?> Login(string usuario, string pass)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(pass))
            {
                throw new ArgumentException("Usuario y password son obligatorios.");
            }

            return await _context.UsuariosSistema
                .AsNoTracking()
                .Where(u => u.Usuario == usuario.Trim() && u.Pass == pass.Trim())
                .Select(u => (object)new
                {
                    id = u.Id,
                    nombre = $"{u.Nombres} {u.Apellidos}".Trim(),
                    usuario = u.Usuario,
                    rol = u.Rol
                })
                .FirstOrDefaultAsync();
        }
    }
}
