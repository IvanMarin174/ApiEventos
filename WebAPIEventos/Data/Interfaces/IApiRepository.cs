using WebAPIEventos.Models;

namespace WebAPIEventos.Data.Interfaces
{
    public interface IApiRepository
    {
 
       Task<IEnumerable<Evento>> GetEventosAsync();
       Task<Evento> GetEventoByAsync(int id);
       

    }
}
