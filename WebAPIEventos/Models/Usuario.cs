namespace WebAPIEventos.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Correo {  get; set; }
        public string Password { get; set; }
        public required int RolId { get; set; }

        public virtual Rol? Rol { get; set;}//Un usuario tiene un rol

        public virtual List<Evento>? Eventos { get; set; }// Un usario tiene mas de un evento(One-to-Many)

    }
}
