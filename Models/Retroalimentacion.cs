using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //Necesario para el uso de [Key]
using System.ComponentModel.DataAnnotations.Schema; //Necesario para el uso de [DatabaseGenerated]

namespace Jham.Models
{
    public class Retroalimentacion
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Comentario { get; set; }
    public int Calificacion { get; set; }
    public DateTime Fecha { get; set; }

    public int CasoId { get; set; }
    public Caso Caso { get; set; }
    }
}
