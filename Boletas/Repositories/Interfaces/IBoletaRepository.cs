using Boletas.DTOs;

namespace Boletas.Repositories.Interfaces
{
    public interface IBoletaRepository
    {
        Task InsertBoleta(BoletaDto payload);
    }
}