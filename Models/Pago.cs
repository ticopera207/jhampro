using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jhampro.Models
{
    [Table("pago")]
    public class Pago
    {
        public int Id { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string Metodo { get; set; } = null!;

        // Relación uno a uno (Pago a Servicio)
        public int ServicioId { get; set; }
        public Servicio Servicio { get; set; } = null!;
    }
}