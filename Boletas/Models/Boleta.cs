using System.ComponentModel.DataAnnotations.Schema;

namespace Boletas.Models
{
    [Table("Boleta")]
    public class Boleta
    {
        public int Id { get; set; }
        public int IdTraslado { get; set; }
        public int IdTurno { get; set; }
        public int IdUnidad { get; set; }
        public int IdOperador { get; set; }
        public int IdPlanta { get; set; }
        public DateTime TsCarga { get; set; }
        public string HoraEntrada { get; set; } = null!;
        public string HoraSalida { get; set; } = null!;
        public string NombreSupervisor { get; set; } = null!;
        public string NombreSupervisorPlanta { get; set; } = null!;
        public string Folio { get; set; } = null!;
    }
}