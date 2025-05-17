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

        public IActionResult Agendado()
        {
            ViewBag.Usuario = _context.Usuarios
                .Select(a => new { a.Id, Nombre = a.Nombres + " " + a.Apellidos })
                .ToList();
            return View();
        }


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
        }
    }
}