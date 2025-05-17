using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //Necesario para el uso de [Key]
using System.ComponentModel.DataAnnotations.Schema; //Necesario para el uso de [DatabaseGenerated]

namespace Jham.Models
{
    public class Documento
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
    public int Id { get; set; }
    public string NombreArchivo { get; set; }
    public string RutaArchivo { get; set; }
    public DateTime FechaSubida { get; set; }
    public string? Observacion { get; set; }

    public int CasoId { get; set; }
    public Caso Caso { get; set; }
    
    }
}