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

        public IActionResult Agendado()
        {
            ViewBag.Abogados = _context.Abogados
                .Select(a => new { a.Id, Nombre = a.Usuario.Nombre + " " + a.Usuario.Apellidos })
                .ToList();
            return View();
        }


        //CORREGIR ESTE MÉTODO
        [HttpPost]
        public IActionResult RegistrarCita(Cita cita)
        {
            if (ModelState.IsValid)
            {
                _context.Citas.Add(cita);
                _context.SaveChanges();
                return RedirectToAction("","");
            }

            ViewBag.Abogados = _context.Abogados
                .Select(a => new { a.Id, Nombre = a.Usuario.Nombre + " " + a.Usuario.Apellidos })
                .ToList();
            return View("Agendado", cita);
        }
    }
}