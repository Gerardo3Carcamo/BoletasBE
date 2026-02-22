using System.ComponentModel.DataAnnotations.Schema;
namespace Boletas.Models
{
    [Table("Operador")]
    public class Operador
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}