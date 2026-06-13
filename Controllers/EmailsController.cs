using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Domain.DTOs.WebRequest.Email;

namespace sapo_clone.API.Controllers
{
    [Route("api/emails")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailsController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-mail")]
        public async Task<IActionResult> SendEmail([FromBody] ForgotPasswordEmailDto dto)
        {
            var emailResult = await _emailService.SendTransactionalEmailAsync(
                dto.ToEmail,
                dto.Subject,
                dto.HtmlBody
            );

            if (emailResult.Success)
            {
                // Email gửi thành công, emailResult.Data chứa MessageId
                return Ok(emailResult);
            }
            else
            {
                // Gửi email thất bại, emailResult.Errors chứa danh sách lỗi
                // Bạn có thể log các lỗi này hoặc trả về cho client
                return BadRequest(emailResult);
            }
        }

        [HttpPost("test-configuration")]
        public async Task<IActionResult> TestEmailConfiguration([FromBody] TestEmailDto dto)
        {
            var emailResult = await _emailService.TestEmailConfigurationAsync(dto.TestEmail);

            if (emailResult.Success)
            {
                return Ok(emailResult);
            }
            else
            {
                return BadRequest(emailResult);
            }
        }
    }
}
