using Microsoft.AspNetCore.Mvc;
using Jham.Models;

namespace Jham.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario usuario)
        {
            DateTime hoy = DateTime.Today;
         int edad = hoy.Year - usuario.FechaNacimiento.Year;
            if (usuario.FechaNacimiento.Date > hoy.AddYears(-edad)) edad--;

         if (edad < 18)
            {
                ModelState.AddModelError("FechaNacimiento", "Debes tener al menos 18 años para registrarte.");
             }

         if (!ModelState.IsValid)
            {
                return View(usuario);
            }
            usuario.FechaNacimiento = DateTime.SpecifyKind(usuario.FechaNacimiento, DateTimeKind.Utc);
             _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
             return RedirectToAction("RegistroExitoso");
        }

        public IActionResult RegistroExitoso()
        {
            return View();
        }
    }
}
