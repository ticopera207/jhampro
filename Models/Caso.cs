using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //Necesario para el uso de [Key]
using System.ComponentModel.DataAnnotations.Schema; //Necesario para el uso de [DatabaseGenerated]

namespace Jham.Models
{
    public class Caso
    {
      [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string EstadoCaso { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    public int CitaId { get; set; }
    public Cita Cita { get; set; }

    public ICollection<Documento> Documentos { get; set; }
    public ICollection<Retroalimentacion> Retroalimentaciones { get; set; }
    }
}
