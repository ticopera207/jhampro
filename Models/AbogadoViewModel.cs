using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jhampro.Models
{
    public class AbogadoViewModel
    {
        public List<Usuario> Usuarios { get; set; }
        public string UsuarioNombre { get; set; }
        public string apellidosAbogado { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
    }
    
}