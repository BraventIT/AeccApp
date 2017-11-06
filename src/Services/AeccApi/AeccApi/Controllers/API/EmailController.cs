using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AeccApi.Models;
using AeccApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AeccApi.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Email")]
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly EmailData _emailData;

        public EmailController(IEmailService emailService, IOptions<EmailData> options)
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