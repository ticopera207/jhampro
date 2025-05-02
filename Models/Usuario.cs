using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //Necesario para el uso de [Key]
using System.ComponentModel.DataAnnotations.Schema; //Necesario para el uso de [DatabaseGenerated]


namespace Jham.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Clave primaria

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; } // Correo electrónico

        [Required]
        [MinLength(6)]
        public string Contraseña { get; set; } // Contraseña del usuario

        [Required]
        [Phone]
        public string Celular { get; set; } // Número de celular

        [Required]
        [StringLength(8, MinimumLength = 8)]
        public string Dni { get; set; } // DNI del usuario

        [Required]
        public string TipoUsuario { get; set; }
        
        [Range(0, 120)]
        public int Edad { get; set; } // Edad del usuario
    }
}
