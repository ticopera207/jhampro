using System.ComponentModel.DataAnnotations;

namespace jhampro.Models
{
    public class RecuperarContrasenaViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido.")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string NuevaContrasena { get; set; }

        [Required(ErrorMessage = "Debes repetir la nueva contraseña.")]
        [Compare("NuevaContrasena", ErrorMessage = "Las contraseñas no coinciden.")]
        [DataType(DataType.Password)]
        public string RepetirContrasena { get; set; }
    }

}
