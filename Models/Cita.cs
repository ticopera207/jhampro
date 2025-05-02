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

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public Caso Caso { get; set; }
    }
}