using Microsoft.AspNetCore.Mvc;
using jhampro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace jhampro.Controllers
{
    public class DocumentoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _s3Client;


        // Vista Perfil con acceso a Ver y Gestionar
public IActionResult Perfil()
{
    return View();
}

        public DocumentoController(ApplicationDbContext context, IConfiguration configuration, IAmazonS3 s3Client)
        {
            _context = context;
            _configuration = configuration;
            _s3Client = s3Client;
        }

        // ✅ Ver documentos
        public async Task<IActionResult> Ver()
        {
            var documentos = await _context.Documentos.OrderByDescending(d => d.FechaSubida).ToListAsync();
            return View(documentos);
        }

        // ✅ Vista para subir documentos (GET)
        public IActionResult Gestionar()
        {
            return View();
        }

        // ✅ Subida de documentos (POST)
        [HttpPost]
        public async Task<IActionResult> Gestionar(Documento model, IFormFile Archivo)
        {
            if (Archivo == null || Archivo.Length == 0)
            {
                ModelState.AddModelError("Archivo", "Por favor, selecciona un archivo válido.");
                return View(model);
            }

            var bucketName = _configuration["AWS:BucketName"];
            var fileName = $"{Guid.NewGuid()}_{Archivo.FileName}";

            using (var newMemoryStream = new MemoryStream())
            {
                await Archivo.CopyToAsync(newMemoryStream);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = fileName,
                    BucketName = bucketName,
                    ContentType = Archivo.ContentType,
                   //  CannedACL = S3CannedACL.PublicRead // Para que se pueda acceder vía URL
                };

                var transferUtility = new TransferUtility(_s3Client);
                await transferUtility.UploadAsync(uploadRequest);
            }

            model.NombreArchivo = Archivo.FileName;
            model.RutaArchivo = $"https://{bucketName}.s3.amazonaws.com/{fileName}";
            model.FechaSubida = DateTime.UtcNow;

            _context.Documentos.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Documento subido correctamente.";
            return RedirectToAction("Ver");
        }

        // ✅ Eliminar documento
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null)
            {
                return NotFound();
            }

            // Eliminar del bucket S3
            var key = documento.RutaArchivo.Split('/').Last();
            await _s3Client.DeleteObjectAsync(_configuration["AWS:BucketName"], key);

            // Eliminar de la base de datos
            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Documento eliminado correctamente.";
            return RedirectToAction("Ver");
        }
    }
}
