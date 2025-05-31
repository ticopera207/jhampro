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
    public class AgendadoCasoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PayPalClientFactory _payPalClientFactory;

        public AgendadoCasoController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _payPalClientFactory = new PayPalClientFactory(configuration);
        }

        [HttpGet]
        public IActionResult Agendado()
        {
            int? clienteId = HttpContext.Session.GetInt32("UsuarioId");

             var model = new AgendadoCasoViewModel
                {
                    Clientes = _context.Usuarios
                        .Where(u => u.TipoUsuario == "Cliente")
                        .ToList(),
                    Casos = _context.Servicios
                        .Where(s => s.ClienteId == clienteId)
                        .Include(s => s.Pago)
                        .ToList()
                };
            return View(model);
        }

        [HttpPost]
        public IActionResult RegistrarCaso(int AbogadoId, DateTime Fecha, int Hora)
        {
            int? clienteId = HttpContext.Session.GetInt32("UsuarioId");
            if (clienteId == null)
                return RedirectToAction("Login", "Login");

            var localDateTime = new DateTime(Fecha.Year, Fecha.Month, Fecha.Day, Hora, 0, 0, DateTimeKind.Unspecified);
            var peruTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            var fechaInicio = TimeZoneInfo.ConvertTimeToUtc(localDateTime, peruTimeZone);
            var fechaFin = fechaInicio.AddHours(1);

            var caso = new Servicio
            {
                Estado = "EnEspera",
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                TipoServicio = "Caso",
                ClienteId = clienteId.Value
            };

            _context.Servicios.Add(caso);
            _context.SaveChanges();

            var abogadoServicio = new AbogadoServicio
            {
                UsuarioId = AbogadoId,
                ServicioId = caso.Id
            };
            _context.AbogadoServicio.Add(abogadoServicio);
            _context.SaveChanges();

            TempData["MensajeExito"] = "El caso fue agendado exitosamente.";
            return RedirectToAction("Agendado");
        }

        [HttpGet]
        public IActionResult EditarCaso(int servicioId)
        {
            var caso = _context.Servicios
                .Include(s => s.AbogadoServicios)
                .FirstOrDefault(s => s.Id == servicioId && s.Estado == "EnEspera");

            if (caso == null) return NotFound();

            ViewBag.Abogados = _context.Usuarios.Where(u => u.TipoUsuario == "Abogado").ToList();
            return View(caso);
        }

        [HttpPost]
        public IActionResult EditarCaso(int Id, int AbogadoId, DateTime Fecha, int Hora)
        {
            var caso = _context.Servicios
                .Include(s => s.AbogadoServicios)
                .FirstOrDefault(s => s.Id == Id && s.Estado == "EnEspera");

            if (caso == null) return NotFound();

            var localDateTime = new DateTime(Fecha.Year, Fecha.Month, Fecha.Day, Hora, 0, 0, DateTimeKind.Unspecified);
            var peruTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            var fechaInicio = TimeZoneInfo.ConvertTimeToUtc(localDateTime, peruTimeZone);
            var fechaFin = fechaInicio.AddHours(1);

            caso.FechaInicio = fechaInicio;
            caso.FechaFin = fechaFin;

            var abogadoServicio = caso.AbogadoServicios.FirstOrDefault();
            if (abogadoServicio != null)
                abogadoServicio.UsuarioId = AbogadoId;

            _context.SaveChanges();
            TempData["MensajeExito"] = "Caso editado correctamente.";
            return RedirectToAction("Agendado");
        }

        [HttpPost]
        public IActionResult CancelarCaso(int servicioId)
        {
            var caso = _context.Servicios.FirstOrDefault(s => s.Id == servicioId && s.Estado == "EnEspera");

            if (caso == null)
            {
                TempData["MensajeError"] = "No se pudo cancelar el caso.";
                return RedirectToAction("Agendado");
            }

            caso.Estado = "Cancelado";
            _context.SaveChanges();

            TempData["MensajeExito"] = "El caso fue cancelado correctamente.";
            return RedirectToAction("Agendado");
        }

        [HttpPost]
        public async Task<IActionResult> PagarCaso(int servicioId)
        {
            var caso = _context.Servicios
                .Include(s => s.Cliente)
                .FirstOrDefault(s => s.Id == servicioId);

            if (caso == null || caso.Estado != "EnEspera")
                return NotFound();

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
                    ReturnUrl = Url.Action("ConfirmarPago", "AgendadoCaso", new { servicioId }, Request.Scheme),
                    CancelUrl = Url.Action("CancelarPago", "AgendadoCaso", new { servicioId }, Request.Scheme)
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
                var caso = _context.Servicios.FirstOrDefault(s => s.Id == servicioId);
                if (caso != null)
                {
                    caso.Estado = "Pagado";
                    caso.Pago = new Pago
                    {
                        FechaPago = caso.FechaInicio,
                        Monto = 50.00m,
                        Metodo = "PayPal",
                        ServicioId = caso.Id
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
            var caso = _context.Servicios
                .Include(s => s.Cliente)
                .FirstOrDefault(s => s.Id == servicioId && s.Estado == "Pagado");

            if (caso == null)
                return NotFound("Caso no encontrado o aún no pagado.");

            var comprobantesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "comprobantes");
            Directory.CreateDirectory(comprobantesDir);

            var pdfPath = Path.Combine(comprobantesDir, $"Comprobante_Caso_{caso.Id}.pdf");

            if (!System.IO.File.Exists(pdfPath))
            {
                using (var doc = new PdfDocument())
                {
                    var page = doc.AddPage();
                    var gfx = XGraphics.FromPdfPage(page);

                    var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);
                    var fontText = new XFont("Arial", 12, XFontStyle.Regular);

                    gfx.DrawString("COMPROBANTE DE PAGO - CASO LEGAL", fontTitle, XBrushes.Black,
                        new XRect(0, 30, page.Width, 40), XStringFormats.TopCenter);

                    int y = 100;
                    int spacing = 30;

                    gfx.DrawString($"ID del Caso: {caso.Id}", fontText, XBrushes.Black, 50, y); y += spacing;
                    gfx.DrawString($"Fecha de Pago: {caso.FechaInicio:yyyy-MM-dd HH:mm}", fontText, XBrushes.Black, 50, y); y += spacing;
                    gfx.DrawString($"Monto: $50.00", fontText, XBrushes.Black, 50, y); y += spacing;
                    gfx.DrawString($"Método: PayPal", fontText, XBrushes.Black, 50, y); y += spacing;
                    gfx.DrawString($"Cliente ID: {caso.ClienteId}", fontText, XBrushes.Black, 50, y);

                    doc.Save(pdfPath);
                }
            }

            var pdfUrl = Url.Content($"~/comprobantes/Comprobante_Caso_{caso.Id}.pdf");
            return Redirect(pdfUrl);
        }
    }
}
