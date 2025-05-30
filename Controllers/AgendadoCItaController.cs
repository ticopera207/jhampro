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
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace jhampro.Controllers
{
    public class AgendadoCitaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PayPalClientFactory _payPalClientFactory;

        public AgendadoCitaController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _payPalClientFactory = new PayPalClientFactory(configuration);
        }

        [HttpGet]
        public IActionResult Agendado()
        {
            int? clienteId = HttpContext.Session.GetInt32("UsuarioId");

            var model = new AgendadoCitaViewModel
            {
                Abogados = _context.Usuarios
                    .Where(u => u.TipoUsuario == "Abogado")
                    .ToList(),
                Servicios = _context.Servicios
                    .Where(s => s.ClienteId == clienteId)
                    .Include(s => s.Pago)
                    .ToList()
                    };
            return View(model);
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

        [HttpPost]
        public async Task<IActionResult> PagarCita(int servicioId)
        {
            var servicio = _context.Servicios
                .Include(s => s.Cliente)
                .FirstOrDefault(s => s.Id == servicioId);

            if (servicio == null || servicio.Estado != "EnEspera")
            {
                return NotFound();
            }

            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(new OrderRequest
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "USD",
                            Value = "50.00"
                        }
                    }
                },
                ApplicationContext = new ApplicationContext
                {
                    ReturnUrl = Url.Action("ConfirmarPago", "AgendadoCita", new { servicioId }, Request.Scheme),

                    CancelUrl = Url.Action("CancelarPago", "AgendadoCita", new { servicioId }, Request.Scheme)
                }
                
            });

            var client = _payPalClientFactory.CreateClient();
            var response = await client.Execute(request);
            var result = response.Result<Order>();

            var approvalLink = result.Links.FirstOrDefault(link => link.Rel == "approve")?.Href;

            return Redirect(approvalLink);
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmarPago(int servicioId, string token)
        {
            var client = _payPalClientFactory.CreateClient();

            var request = new OrdersCaptureRequest(token);
            request.RequestBody(new OrderActionRequest());

            var response = await client.Execute(request);
            var result = response.Result<Order>();

            if (result.Status == "COMPLETED")
            {
                var servicio = _context.Servicios.FirstOrDefault(s => s.Id == servicioId);
                if (servicio != null)
                {
                    servicio.Estado = "Pagado";
                    servicio.Pago = new Pago
                    {
                        FechaPago = servicio.FechaInicio,
                        Monto = 50.00m,
                        Metodo = "PayPal",
                        ServicioId = servicio.Id
                    };
                    _context.SaveChanges();
                }

                // Generar comprobante PDF aquí (ver siguiente paso)
                var comprobantesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "comprobantes");
                Directory.CreateDirectory(comprobantesDir);

                // Ruta del archivo PDF
                var pdfPath = Path.Combine(comprobantesDir, $"Comprobante_{servicio.Id}.pdf");

                // Crear PDF
                using (var writer = new PdfWriter(pdfPath))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);
                        document.Add(new Paragraph("COMPROBANTE DE PAGO"));
                        document.Add(new Paragraph($"Servicio ID: {servicio.Id}"));
                        document.Add(new Paragraph($"Fecha de Pago: {servicio.FechaInicio.ToString("yyyy-MM-dd HH:mm")}"));
                        document.Add(new Paragraph($"Monto: $50.00"));
                        document.Add(new Paragraph($"Método: PayPal"));
                        document.Add(new Paragraph($"Cliente ID: {servicio.ClienteId}"));
                        document.Close();
                    }
                }

                TempData["MensajeExito"] = "Pago realizado exitosamente.";
                return RedirectToAction("Agendado");
            }

            TempData["MensajeError"] = "Error al procesar el pago.";
            return RedirectToAction("Agendado");
        }

        

        [HttpGet]
        public IActionResult CancelarPago(int servicioId)
        {
            TempData["MensajeError"] = "El pago fue cancelado.";
            return RedirectToAction("Agendado");
        }

    }
}