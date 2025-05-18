using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using jhampro.Models;
using System.Linq;

namespace jhampro.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ApplicationDbContext _context;

        public LoginController(ILogger<LoginController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Correo == Email && u.Contrasena == Password);

            if (usuario != null)
            {
                // Aquí puedes guardar información en la sesión si deseas
            HttpContext.Session.SetString("UsuarioNombre", usuario.Nombres);
            HttpContext.Session.SetString("TipoUsuario", usuario.TipoUsuario);
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
                return RedirectToAction("Index", "Home"); // Redirige a tu página principal
            }

            ViewBag.Error = "Correo o contraseña incorrectos.";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
        public IActionResult Logout()
        {
        HttpContext.Session.Clear(); // Elimina todas las variables de sesión
        return RedirectToAction("Index", "Home");
        }
    }
}
