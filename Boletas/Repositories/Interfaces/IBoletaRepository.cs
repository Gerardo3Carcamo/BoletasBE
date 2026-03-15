using Boletas.DTOs;
namespace Boletas.Repositories.Interfaces
{
    public interface IBoletaRepository
    {
        Task<object> InsertBoleta(BoletaDto payload);
        Task<object> GetBoleta(int id);
        Task<IReadOnlyList<object>> GetBoletas();
    }
}
