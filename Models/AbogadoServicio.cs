using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jhampro.Models
{
    [Table("abogado_servicio")]
    public class AbogadoServicio
    {
        // Claves Primarias Compuestas
        public int UsuarioId { get; set; }
        public Usuario Abogado { get; set; }

        public int ServicioId { get; set; }
        public Servicio Servicio { get; set; }
    }
}