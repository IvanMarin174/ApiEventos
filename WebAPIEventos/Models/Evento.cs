namespace WebAPIEventos.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Descripcion { get; set; }
        public required string Lugar { get; set; }
        public required DateTime? Fecha { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }//Mas de un evento tiene un usuario (Many-to-One)
    }
}
