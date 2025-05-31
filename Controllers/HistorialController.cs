using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using jhampro.Models;


namespace jhampro.Controllers
{
    // [Route("[controller]")]
    public class HistorialController : Controller
    {
        // private readonly ILogger<HistorialController> _logger;
        private readonly ApplicationDbContext _context;

        public HistorialController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Citas()
        {
            int? clienteId = HttpContext.Session.GetInt32("UsuarioId");

            if (clienteId == null)
                return RedirectToAction("Login", "Login");

            var citas = _context.Servicios
                .Include(s => s.Pago)
                .Include(s => s.AbogadoServicios)
                    .ThenInclude(asg => asg.Abogado)
                .Where(s => s.ClienteId == clienteId)
                .OrderByDescending(s => s.FechaInicio)
                .ToList();

            return View(citas);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}