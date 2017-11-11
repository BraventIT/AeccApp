using System.Threading.Tasks;
using AeccApi.Models;
using AeccApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Aecc.Models;

namespace AeccApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Email")]
    //[Authorize]
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly EmailOptions _emailData;

        public EmailController(IEmailService emailService, IOptions<EmailOptions> options)
        {
            _emailService = emailService;
            _emailData = options.Value;
        }

        // POST: api/Email
        [HttpPost]
        public async Task<IActionResult> PostEmail([FromBody] EmailMessage emailMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _emailService.SendAsync(_emailData, emailMessage);

            return NoContent(); 
        }
    }
}