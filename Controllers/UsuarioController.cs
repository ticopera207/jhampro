using Microsoft.AspNetCore.Mvc;
using jhampro.Models;
using System.Text.Json;

namespace jhampro.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            var usuario = new Usuario { TipoUsuario = "Cliente" };
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                // Diagnóstico: muestra los errores en la consola
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Error en {key}: {error.ErrorMessage}");
                    }
                }
                return View(usuario);
            }
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction("RegistroExitoso");
        }

        public IActionResult RegistroExitoso()
        {
            return View();
        }

        //  AQUÍ ESTÁ LA NUEVA ACCIÓN
        [HttpGet]
public async Task<JsonResult> VerificarDni(string dni)
{
    var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJlbWFpbCI6InRpY29wZXJhMjNAaG90bWFpbC5jb20ifQ.zzuYFqeDcZnjQOFrIvcM9ornQUdjU9g2xtoSd0EUa0w"; // cámbialo por el correcto

    var url = $"https://dniruc.apisperu.com/api/v1/dni/{dni}?token={token}";

    using var httpClient = new HttpClient();

    try
    {
        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return Json(new { error = $"API Error: {response.StatusCode}", content });
        }

        var datos = JsonSerializer.Deserialize<JsonElement>(content);

        return Json(new
        {
            nombres = datos.GetProperty("nombres").GetString(),
            apellidoPaterno = datos.GetProperty("apellidoPaterno").GetString(),
            apellidoMaterno = datos.GetProperty("apellidoMaterno").GetString()
        });
    }
    catch (Exception ex)
    {
        return Json(new { error = ex.Message, stack = ex.StackTrace });
    }
}

        
    }
}
