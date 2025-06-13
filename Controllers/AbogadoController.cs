using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace jhampro.Controllers
{
    public class AbogadoController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        public AbogadoController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        public IActionResult Abogado()
        {
            // Validar si el usuario ha iniciado sesión y es Administrador
            var tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

            if (string.IsNullOrEmpty(tipoUsuario) || tipoUsuario != "Abogado")
            {
                return View();
            }

            // Puedes enviar datos a la vista con ViewBag o un modelo
            ViewBag.UsuarioNombre = HttpContext.Session.GetString("UsuarioNombre");
            ViewBag.apellidosAbogado=HttpContext.Session.GetString("apellidosAbogado");
            ViewBag.Celular=HttpContext.Session.GetString("Celular");
            ViewBag.Correo=HttpContext.Session.GetString("Correo");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}