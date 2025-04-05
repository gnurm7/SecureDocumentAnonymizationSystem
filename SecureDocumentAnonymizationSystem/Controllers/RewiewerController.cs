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
    public class ReviewerController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ReviewerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddReviewer([FromBody] Reviewer model)
        {
            if (string.IsNullOrWhiteSpace(model.Email)) return BadRequest("Email zorunlu");

            var exists = await _dbContext.Reviewers.AnyAsync(r => r.Email == model.Email);
            if (exists) return BadRequest("Bu email zaten kayıtlı.");

            _dbContext.Reviewers.Add(model);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Hakem eklendi" });
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllReviewers()
        {
            var list = await _dbContext.Reviewers.ToListAsync();
            return Ok(list);
        }
    }
}