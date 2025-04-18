using System.ComponentModel.DataAnnotations;

namespace Jham.Models
{
    public class Usuario
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [Range(0, 120)]
        public int Edad { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 8)]
        public string Dni { get; set; }
    }
}
