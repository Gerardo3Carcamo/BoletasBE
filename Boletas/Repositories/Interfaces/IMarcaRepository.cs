using Boletas.Models;

namespace Boletas.Repositories.Interfaces
{
    public interface IMarcaRepository
    {
        Task<IReadOnlyList<Marca>> GetMarcas();
    }
}
