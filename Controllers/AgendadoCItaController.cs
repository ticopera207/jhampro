using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using jhampro.Models;

namespace jhampro.Controllers
{
    public class AgendadoCitaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgendadoCitaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Agendado()
        {
            ViewBag.Abogados = _context.Usuarios
                .Where(u => u.TipoUsuario == "Abogado")
                .Select(a => new { a.Id, Nombre = a.Nombres + " " + a.Apellidos })
                .ToList();
            return View();
        }

        /*
        //CORREGIR ESTE MÉTODO
        [HttpPost]
        public IActionResult RegistrarCita(Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                _context.Servicios.Add(servicio);
                _context.SaveChanges();
                return RedirectToAction("","");
            }

            ViewBag.Abogados = _context.Usuarios
                .Select(a => new { a.Id, Nombre = a.Nombres + " " + a.Apellidos })
                .ToList();
            return View("Agendado", servicio);
        }*/

     [HttpPost]
    public IActionResult RegistrarCita(int AbogadoId, DateTime Fecha, int Hora)
    {
        int? clienteId = HttpContext.Session.GetInt32("UsuarioId");
        if (clienteId == null)
            return RedirectToAction("Login", "Login");

        // Construye la fecha local (Perú, UTC-5)
        var localDateTime = new DateTime(Fecha.Year, Fecha.Month, Fecha.Day, Hora, 0, 0, DateTimeKind.Unspecified);

        // Convierte a UTC
        var peruTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"); // Windows
        // Si usas Linux, usa: "America/Lima"
        var fechaInicio = TimeZoneInfo.ConvertTimeToUtc(localDateTime, peruTimeZone);
        var fechaFin = fechaInicio.AddHours(1);

        var servicio = new Servicio
        {
            Estado = "EnEspera",
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            TipoServicio = "Cita",
            ClienteId = clienteId.Value
        };

        _context.Servicios.Add(servicio);
        _context.SaveChanges();

        var abogadoServicio = new AbogadoServicio
        {
            UsuarioId = AbogadoId,
            ServicioId = servicio.Id
        };
        _context.AbogadoServicio.Add(abogadoServicio);
        _context.SaveChanges();
        TempData["MensajeExito"] = "La cita fue agendada exitosamente."; //Mensaje de exito
        return RedirectToAction("Agendado");
    }
    }
}