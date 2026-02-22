using System.ComponentModel.DataAnnotations.Schema;
namespace Boletas.Models
{
    [Table("Traslado")]
    public class Traslado
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = null!;
    }
}