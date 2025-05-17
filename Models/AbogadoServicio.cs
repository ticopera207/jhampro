using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jhampro.Models
{
    public class AbogadoServicio
    {
         // Claves Primarias Compuestas
        public int UsuarioId { get; set; }
        public Usuario Abogado { get; set; }

        public int ServicioId { get; set; }
        public Servicio Servicio { get; set; }
    }
}