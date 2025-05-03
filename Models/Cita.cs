using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jham.Models;
using System.ComponentModel.DataAnnotations; //Necesario para el uso de [Key]
using System.ComponentModel.DataAnnotations.Schema; //Necesario para el uso de [DatabaseGenerated]
namespace Jham.Models

{
  public class Cita
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string EstadoCita { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan Hora { get; set; }

    [ForeignKey("Usuario")]
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } // Por ejemplo: cliente

    [ForeignKey("Usuario2")]
    public int UsuarioId2 { get; set; }
    public Usuario Usuario2 { get; set; } // Por ejemplo: quien atiende la cita

    [ForeignKey("Abogado")]
    public int AbogadoId { get; set; }
    public Abogado Abogado { get; set; }

    public Caso Caso { get; set; } // Si aquí también hay relación, considera añadir ForeignKey
}

}