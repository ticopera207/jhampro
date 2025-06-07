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
// using iText.IO.Font.Constants;
// using iText.Kernel.Font;
// using iText.Kernel.Pdf;
// using iText.Layout;
// using iText.Layout.Element;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

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
            TempData["CasoOK"] = "Caso OK correcto";
            
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

        [HttpGet]
        public IActionResult EditarCita(int servicioId)
        {
            var servicio = _context.Servicios
                .Include(s => s.AbogadoServicios)
                .FirstOrDefault(s => s.Id == servicioId && s.Estado == "EnEspera");
            if (servicio == null)
                return NotFound();

            var abogados = _context.Usuarios.Where(u => u.TipoUsuario == "Abogado").ToList();
            ViewBag.Abogados = abogados;
            return View(servicio);
        }

        [HttpPost]
        public IActionResult EditarCita(int Id, int AbogadoId, DateTime Fecha, int Hora)
        {
            var servicio = _context.Servicios
                .Include(s => s.AbogadoServicios)
                .FirstOrDefault(s => s.Id == Id && s.Estado == "EnEspera");
            if (servicio == null)
                return NotFound();

            // Actualizar fecha/hora
            var localDateTime = new DateTime(Fecha.Year, Fecha.Month, Fecha.Day, Hora, 0, 0, DateTimeKind.Unspecified);
            var peruTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            var fechaInicio = TimeZoneInfo.ConvertTimeToUtc(localDateTime, peruTimeZone);
            var fechaFin = fechaInicio.AddHours(1);

            servicio.FechaInicio = fechaInicio;
            servicio.FechaFin = fechaFin;

            // Actualizar abogado
            var abogadoServicio = servicio.AbogadoServicios.FirstOrDefault();
            if (abogadoServicio != null)
                abogadoServicio.UsuarioId = AbogadoId;

            _context.SaveChanges();
            TempData["MensajeExito"] = "Cita editada correctamente.";
            return RedirectToAction("Agendado");
        }

        [HttpPost]
        public IActionResult CancelarCita(int servicioId)
        {
            var servicio = _context.Servicios.FirstOrDefault(s => s.Id == servicioId && s.Estado == "EnEspera");
            if (servicio == null)
            {
                TempData["MensajeError"] = "No se pudo cancelar la cita.";
                return RedirectToAction("Agendado");
            }

            servicio.Estado = "Cancelado";
            _context.SaveChanges();

            TempData["MensajeExito"] = "La cita fue cancelada correctamente.";
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


        [HttpGet]
        public IActionResult GenerarComprobante(int servicioId)
        {
            var servicio = _context.Servicios
                .Include(s => s.Cliente)
                .FirstOrDefault(s => s.Id == servicioId && s.Estado == "Pagado");

            if (servicio == null)
                return NotFound("Servicio no encontrado o aún no pagado.");

            var comprobantesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "comprobantes");
            Directory.CreateDirectory(comprobantesDir);

            var pdfPath = Path.Combine(comprobantesDir, $"Comprobante_{servicio.Id}.pdf");

            if (!System.IO.File.Exists(pdfPath))
            {
                using (var doc = new PdfDocument())
                {
                    var page = doc.AddPage();
                    var gfx = XGraphics.FromPdfPage(page);

                    var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);
                    var fontText = new XFont("Arial", 12, XFontStyle.Regular);

                    gfx.DrawString("COMPROBANTE DE PAGO", fontTitle, XBrushes.Black,
                        new XRect(0, 30, page.Width, 40), XStringFormats.TopCenter);

                    int y = 100;
                    int spacing = 30;

                    gfx.DrawString($"Servicio ID: {servicio.Id}", fontText, XBrushes.Black, 50, y); y += spacing;
                    gfx.DrawString($"Fecha de Pago: {servicio.FechaInicio:yyyy-MM-dd HH:mm}", fontText, XBrushes.Black, 50, y); y += spacing;
                    gfx.DrawString($"Monto: $50.00", fontText, XBrushes.Black, 50, y); y += spacing;
                    gfx.DrawString($"Método: PayPal", fontText, XBrushes.Black, 50, y); y += spacing;
                    gfx.DrawString($"Cliente ID: {servicio.ClienteId}", fontText, XBrushes.Black, 50, y);

                    doc.Save(pdfPath);
                }
            }

            var pdfUrl = Url.Content($"~/comprobantes/Comprobante_{servicio.Id}.pdf");
            return Redirect(pdfUrl);
        }
    }
}