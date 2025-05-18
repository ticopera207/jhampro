using Microsoft.AspNetCore.Mvc;
using jhampro.Models;

namespace jhampro.Controllers
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
         if (!ModelState.IsValid)
            {
                return View(usuario);
            }
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
