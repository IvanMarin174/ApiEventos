using Microsoft.EntityFrameworkCore;
using WebAPIEventos.Context;
using WebAPIEventos.Data.Interfaces;
using WebAPIEventos.Models;

namespace WebAPIEventos.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            this._context = context;
        }
        public async Task<bool> ExisteUsuario(string correo)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Correo == correo))
                return true;
            return false;
        }

        public Task<Usuario> Login(string correo, string password)
        {
            throw new NotImplementedException();
        }
    }
}
