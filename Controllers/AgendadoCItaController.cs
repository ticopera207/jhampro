using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Jham.Models;

namespace jhampro.Controllers
{
    public class AgendadoCItaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgendadoCItaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Agendar()
        {
            ViewBag.Abogados = _context.Abogados
                .Select(a => new { a.Id, Nombre = a.Usuario.Nombre + " " + a.Usuario.Apellidos })
                .ToList();
            return View();
        }

        [HttpGet]
        public JsonResult GetHorariosDisponibles(int abogadoId, DateTime fecha)
        {
            var horariosOcupados = _context.Citas
                .Where(c => c.AbogadoId == abogadoId && c.Fecha.Date == fecha.Date)
                .Select(c => c.Hora)
                .ToList();

            var horarios = new List<TimeSpan>();
            for (int hora = 9; hora <= 17; hora++)
            {
                if (hora == 12) continue; // Excluir 12-13 hrs
                var horario = new TimeSpan(hora, 0, 0);
                if (!horariosOcupados.Contains(horario))
                {
                    horarios.Add(horario);
                }
            }

            return Json(horarios);
        }

        [HttpPost]
        public IActionResult RegistrarCita(Cita cita)
        {
            if (ModelState.IsValid)
            {
                _context.Citas.Add(cita);
                _context.SaveChanges();
                return RedirectToAction("Agendar");
            }

            ViewBag.Abogados = _context.Abogados
                .Select(a => new { a.Id, Nombre = a.Usuario.Nombre + " " + a.Usuario.Apellidos })
                .ToList();
            return View("Agendar", cita);
        }
    }
}