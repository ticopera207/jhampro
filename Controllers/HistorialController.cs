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

        [HttpGet]
        public IActionResult Citas(HistorialFiltroViewModel filtro)
        {
            int? clienteId = HttpContext.Session.GetInt32("UsuarioId");

            if (clienteId == null)
                return RedirectToAction("Login", "Login");

            var query = _context.Servicios
                    .Include(s => s.Pago)
                    .Include(s => s.AbogadoServicios)
                        .ThenInclude(asg => asg.Abogado)
                    .Where(s => s.ClienteId == clienteId);
                    
            
            // Aplicar filtros dinámicamente
            if (filtro.Id.HasValue)
                query = query.Where(s => s.Id == filtro.Id.Value);

            if (filtro.Fecha.HasValue)
            {
                var fechaUtc = DateTime.SpecifyKind(filtro.Fecha.Value.Date, DateTimeKind.Utc);
                query = query.Where(s => s.FechaInicio.Date == fechaUtc.Date);
            }
            
            

            var citas = query
            .AsEnumerable() // Aquí pasa a memoria
            .Where(s =>
                string.IsNullOrEmpty(filtro.Hora) || s.FechaInicio.ToString("HH:mm") == filtro.Hora)
            .Where(s =>
                string.IsNullOrEmpty(filtro.Estado) || s.Estado.Contains(filtro.Estado, StringComparison.OrdinalIgnoreCase))
            .Where(s =>
                string.IsNullOrEmpty(filtro.NombreAbogado) || s.AbogadoServicios.Any(a =>
                    (a.Abogado.Nombres + " " + a.Abogado.Apellidos)
                    .Contains(filtro.NombreAbogado, StringComparison.OrdinalIgnoreCase)))
            .Where(s =>
                !filtro.Pagado.HasValue || (s.Pago != null) == filtro.Pagado.Value)
            .OrderByDescending(s => s.FechaInicio)
            .ToList();

        filtro.Resultados = citas;

            return View(filtro);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}