using System.ComponentModel.DataAnnotations.Schema;

namespace Jham.Models
{
    public class Rel_AbogadoCaso
    {
        [ForeignKey("Abogado")]
        public int AbogadoId { get; set; }
        public Abogado Abogado { get; set; }

        [ForeignKey("Caso")]
        public int CasoId { get; set; }
        public Caso Caso { get; set; }
    }
}
