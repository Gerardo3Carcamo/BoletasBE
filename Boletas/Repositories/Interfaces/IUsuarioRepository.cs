namespace Boletas.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IReadOnlyList<object>> GetUsuarios();
        Task<object?> Login(string usuario, string pass);
    }
}
