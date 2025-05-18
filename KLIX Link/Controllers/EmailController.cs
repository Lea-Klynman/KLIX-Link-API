using KLIX_Link_Core.Entities;
using KLIX_Link_Core.IServices;
using KLIX_Link_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KLIX_Link.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }


        [HttpPost("send")]
        public async Task<IActionResult> SendEmailAsync([FromBody] EmailRequest request)
        {
            await _emailService.SendEmailAsync(request);
            return Ok();
        }
        [HttpGet("unread")]
        public async Task<IActionResult> GetUnreadEmails()
        {
            var emails = await _emailService.GetUnreadEmailsAsync();
            return Ok(emails);
        }

        

        [HttpPost("mark-as-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _emailService.MarkEmailAsReadAsync(id);
            return result ? Ok() : NotFound();
        }

        [HttpDelete("{emailId}")]
        public async Task<IActionResult> DeleteEmail(int emailId)
        {
            var success = await _emailService.DeleteEmailAsync(emailId);
            if (success)
                return Ok(new { message = "Email deleted successfully." });
            else
                return NotFound(new { message = "Email not found or could not be deleted." });
        }
    }
}
