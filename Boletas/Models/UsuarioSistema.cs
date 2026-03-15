using System.ComponentModel.DataAnnotations.Schema;

namespace Boletas.Models
{
    [Table("Usuario")]
    public class UsuarioSistema
    {
        public int Id { get; set; }
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Usuario { get; set; } = null!;
        public string Pass { get; set; } = null!;
        public int Rol { get; set; }
    }
}
