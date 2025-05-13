using Microsoft.AspNetCore.Mvc;
using SecureDocumentAnonymizationSystem.Data;
using SecureDocumentAnonymizationSystem.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SecureDocumentAnonymizationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MessageController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        [HttpGet("{trackingNumber}")]
        public IActionResult GetMessages(string trackingNumber)
        {
            var messages = _context.Messages
                .Where(m => m.TrackingNumber == trackingNumber)
                .OrderBy(m => m.Date)
                .ToList();

            return Ok(new { messages });
        }
    }
}
