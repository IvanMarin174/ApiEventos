namespace WebAPIEventos.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public virtual ICollection<Usuario>? Usuario { get; set; }
    }
}
