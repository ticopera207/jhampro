using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jham.Models
{
    public class Especialidad
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public string NombreEspecialidad { get; set; }

        public ICollection<Rel_AbogadoEspecialidad> Abogados { get; set; }
    }
}
