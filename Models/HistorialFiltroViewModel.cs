using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jhampro.Models
{
    public class HistorialFiltroViewModel
    {
    public int? Id { get; set; }
    public DateTime? Fecha { get; set; }
    public string Hora { get; set; }
    public string Estado { get; set; }
    public string NombreAbogado { get; set; }
    public bool? Pagado { get; set; }

    public List<Servicio> Resultados { get; set; } = new();
    }
}