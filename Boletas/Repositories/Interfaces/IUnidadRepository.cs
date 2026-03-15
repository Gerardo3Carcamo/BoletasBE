using Boletas.DTOs;

namespace Boletas.Repositories.Interfaces
{
    public interface IUnidadRepository
    {
        Task<object> CrearUnidad(CrearUnidadDto payload);
        Task<IReadOnlyList<object>> GetUnidades();
    }
}
