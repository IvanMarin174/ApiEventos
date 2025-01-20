using WebAPIEventos.Models;

namespace WebAPIEventos.Data.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> ExisteUsuario(string correo);
        Task<Usuario>Login(string correo, string password);
    }
}
