using Auto1040.Core.DTOs;
using Auto1040.Core.Services;
using Auto1040.Core.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Auto1040.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendContactEmail([FromBody] ContactFormDto dto)
        {
            var emailRequest = new EmailRequest
            {
                To = "m0556777068@gmail.com", 
                Subject = "New Contact Form Submission",
                Body = $"Name: {dto.FullName}\nEmail: {dto.Email}\nMessage:\n{dto.Message}"
            };

            var success = await _emailService.SendEmailAsync(emailRequest);
            if (success)
                return Ok();
            else
                return StatusCode(500, "Failed to send email");
        }
    }

}
