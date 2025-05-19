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
            var usuario = new Usuario { TipoUsuario = "Cliente" };
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                // Diagnóstico: muestra los errores en la consola
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Error en {key}: {error.ErrorMessage}");
                    }
                }
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
