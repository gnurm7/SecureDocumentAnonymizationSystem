using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureDocumentAnonymizationSystem.Data;
using SecureDocumentAnonymizationSystem.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SecureDocumentAnonymizationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly string _uploadPath;

        public FileUploadController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "upload");

            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromForm] string email)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Dosya seçilmedi.");

            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                return BadRequest("Geçerli bir e-posta adresi girmeniz gerekmektedir.");

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_uploadPath, uniqueFileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var trackingNumber = Guid.NewGuid().ToString();

                var makale = new Makale
                {
                    Email = email,
                    FileName = uniqueFileName,
                    FilePath = filePath,
                    UploadDate = DateTime.UtcNow,
                    TrackingNumber = trackingNumber,
                    Status = "Yuklenmis",
                };

                _dbContext.Makaleler.Add(makale);
                await _dbContext.SaveChangesAsync();

                return Ok(new { Message = "Dosya başarıyla yüklendi!", TrackingNumber = trackingNumber });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        [HttpPut("revise/{trackingNumber}")]
        public async Task<IActionResult> ReviseFile(string trackingNumber, IFormFile newFile)
        {
            if (newFile == null || newFile.Length == 0)
                return BadRequest("Yeni dosya seçilmedi.");

            var makale = await _dbContext.Makaleler.FirstOrDefaultAsync(m => m.TrackingNumber == trackingNumber);
            if (makale == null)
                return NotFound("Makale bulunamadı.");

            var newFileName = $"{Guid.NewGuid()}_{newFile.FileName}";
            var newFilePath = Path.Combine(_uploadPath, newFileName);

            try
            {
                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await newFile.CopyToAsync(stream);
                }

                makale.FileName = newFileName;
                makale.FilePath = newFilePath;
                makale.Status = "Revize Edilmiş";
                makale.UploadDate = DateTime.UtcNow;

                _dbContext.Makaleler.Update(makale);
                await _dbContext.SaveChangesAsync();

                return Ok(new { Message = "Makale başarıyla revize edildi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpGet("revise/{trackingNumber}")]
        public async Task<IActionResult> GetRevisedFile(string trackingNumber)
        {
            var makale = await _dbContext.Makaleler
                                         .Where(m => m.TrackingNumber == trackingNumber && m.Status == "Revize Edilmiş")
                                         .FirstOrDefaultAsync();

            if (makale == null)
                return NotFound("Revize edilmiş makale bulunamadı.");

            return Ok(new
            {
                Message = "Revize edilmiş makale başarıyla bulundu.",
                Makale = new
                {
                    makale.Id,
                    makale.Email,
                    makale.FileName,
                    makale.FilePath,
                    makale.UploadDate,
                    makale.TrackingNumber,
                    makale.Status
                }
            });
        }
        [HttpGet("status/{trackingNumber}")]
        public async Task<IActionResult> GetStatus(string trackingNumber, [FromQuery] string email)
        {
            var makale = await _dbContext.Makaleler
                .FirstOrDefaultAsync(m => m.TrackingNumber == trackingNumber && m.Email == email);

            if (makale == null)
                return NotFound("Makale bulunamadı.");

            return Ok(new
            {
                status = makale.Status,
                feedback = makale.Feedback // Eğer varsa, string Feedback alanı
            });
        }


    }
}
