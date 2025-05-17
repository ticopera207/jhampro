using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jhampro.Models
{
    [Table("servicio")]
    public class Servicio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Estado { get; set; } = "EnEspera";
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string TipoServicio { get; set; } = "Cita";  // "Cita" y "Caso"

        // Relación uno a muchos (Cliente a Servicio)
        public int ClienteId { get; set; }
        public Usuario Cliente { get; set; }

        // Relación muchos a muchos (Abogado a Servicio)
        public ICollection<AbogadoServicio> AbogadoServicios { get; set; };
    }
}