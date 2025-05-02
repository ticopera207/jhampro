using Microsoft.AspNetCore.Mvc;
using Jham.Models;

namespace Jham.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrarse(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Aquí guardarías el usuario en la BD o lógica correspondiente
                return RedirectToAction("Privacy");
            }

            return View(usuario);
        }
        
        public IActionResult RegistroExitoso()
        {
            return View();
        }
    }
}
