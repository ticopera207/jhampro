using System.ComponentModel.DataAnnotations.Schema;

namespace Jham.Models
{
    public class Rel_AbogadoEspecialidad
    {
        [ForeignKey("Abogado")]
        public int AbogadoId { get; set; }
        public Abogado Abogado { get; set; }

        [ForeignKey("Especialidad")]
        public int EspecialidadId { get; set; }
        public Especialidad Especialidad { get; set; }
    }
}