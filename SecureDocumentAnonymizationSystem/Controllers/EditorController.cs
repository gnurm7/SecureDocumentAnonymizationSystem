using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureDocumentAnonymizationSystem.Data;
using SecureDocumentAnonymizationSystem.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SecureDocumentAnonymizationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public EditorController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet("articles")]
        public async Task<IActionResult> GetArticles()
        {
            var articles = await _dbContext.Makaleler
                .Select(m => new
                {
                    m.Id,
                    m.Email,
                    m.Status,
                    m.UploadDate,
                    m.TrackingNumber,
                    m.FilePath,
                    m.FileName,
                    m.ReviewerFeedback,
                    m.AnonymizedFileName, 
                    PdfUrl = $"{Request.Scheme}://{Request.Host}/upload/{m.FileName}"
                })
                .ToListAsync();

            return Ok(articles);
        }
       


        [HttpPost("status/update")]
        public async Task<IActionResult> UpdateStatus([FromBody] StatusUpdateRequest model)
        {
            var article = await _dbContext.Makaleler.FindAsync(model.ArticleId);
            if (article == null)
                return NotFound("Makale bulunamadı.");

            article.Status = model.Status;
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Durum güncellendi." });
        }

        [HttpPost("feedback")]
        public async Task<IActionResult> AddFeedback([FromBody] FeedbackRequest model)
        {
            var article = await _dbContext.Makaleler.FindAsync(model.ArticleId);
            if (article == null)
                return NotFound("Makale bulunamadı.");

            article.Feedback = model.Feedback;
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Geri bildirim eklendi." });
        }

        public class StatusUpdateRequest
        {
            public int ArticleId { get; set; }
            public string Status { get; set; }
        }

        public class FeedbackRequest
        {
            public int ArticleId { get; set; }
            public string Feedback { get; set; }
        }

        [HttpPost("assign-reviewer")]
        public async Task<IActionResult> AssignReviewer([FromBody] AssignReviewerRequest model)
        {
            var article = await _dbContext.Makaleler.FindAsync(model.ArticleId);
            if (article == null)
                return NotFound("Makale bulunamadı.");

            if (!string.IsNullOrEmpty(article.ReviewerEmail))
                return BadRequest("Bu makaleye zaten bir hakem atanmış.");

            article.ReviewerEmail = model.ReviewerEmail;
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Hakem başarıyla atandı." });
        }


        public class AssignReviewerRequest
        {
            public int ArticleId { get; set; }
            public string ReviewerEmail { get; set; }
        }

        [HttpGet("reviewer/articles")]
        public async Task<IActionResult> GetReviewerArticles([FromQuery] string email)
        {
            var articles = await _dbContext.Makaleler
                .Where(m => m.ReviewerEmail == email && m.AnonymizedFileName != null && m.AnonymizedFileName.EndsWith("_anon.pdf"))
                .Select(m => new
                {
                    m.Id,
                    m.Status,
                    m.TrackingNumber,
                    m.AnonymizedFileName,
                    PdfUrl = $"{Request.Scheme}://{Request.Host}/upload/{m.AnonymizedFileName}"
                })
                .ToListAsync();

            return Ok(articles);
        }
        [HttpPost("review")]
        public async Task<IActionResult> SubmitReview([FromBody] ReviewRequest model)
        {
            var article = await _dbContext.Makaleler.FindAsync(model.ArticleId);
            if (article == null)
                return NotFound("Makale bulunamadı.");

            article.ReviewerFeedback = model.ReviewText;
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Hakem değerlendirmesi kaydedildi." });
        }

        public class ReviewRequest
        {
            public int ArticleId { get; set; }
            public string ReviewText { get; set; }
        }


        [HttpPost("anonymized-pdf")]
        public async Task<IActionResult> SaveAnonymizedPdf([FromBody] AnonymizedPdfModel model)
        {
            var article = await _dbContext.Makaleler.FindAsync(model.ArticleId);
            if (article == null)
                return NotFound("Makale bulunamadı.");

            article.AnonymizedFileName = model.AnonymizedFileName;
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Anonimleştirilmiş PDF kaydedildi." });
        }

        public class AnonymizedPdfModel
        {
            public int ArticleId { get; set; }
            public string AnonymizedFileName { get; set; }
        }


    }
}
