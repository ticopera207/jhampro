using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jhampro.Models
{
    [Table("retroalimentacion")]
    public class Retroalimentacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Clave primaria
        public string Comentario { get; set; }
        public int Calificacion { get; set; }
        public DateTime Fecha { get; set; }
        public bool Publico { get; set; }

        // Relación uno a uno (Retroalimentación a Servicio)
        public int ServicioId { get; set; }
        public Servicio Servicio { get; set; }
    }
}