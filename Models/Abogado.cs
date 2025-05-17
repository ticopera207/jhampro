using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jham.Models
{
    public class Abogado
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<Rel_AbogadoEspecialidad> Especialidades { get; set; }
        public ICollection<Rel_AbogadoCaso> Casos { get; set; }
    }

}
