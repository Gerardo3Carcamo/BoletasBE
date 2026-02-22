using System.ComponentModel.DataAnnotations.Schema;
namespace Boletas.Models
{
    [Table("Planta")]
    public class Planta
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string NombreContacto { get; set; } = null!;
        public string NumeroContacto { get; set; } = null!;
    }
}