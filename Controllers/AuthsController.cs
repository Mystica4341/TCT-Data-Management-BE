using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Domain.DTOs.WebRequest.Auth;
using System.Security.Claims;

namespace sapo_clone.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthsController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var response = await _authService.Login(login);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await _authService.GetCurrentUser(User.FindFirstValue(ClaimTypes.Email));

            return Ok(result);
        }
        [HttpPost("verify-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyAccountOtp([FromBody] VerifyAccountOtpRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _authService.VerifyAccountOtpAsync(request.OtpCode, cancellationToken);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("resend-verification-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendVerificationOtp([FromBody] ResendVerificationOtpRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _authService.ResendVerificationOtpAsync(request.Email);
            if (!response.Success) 
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestPasswordResetOtp([FromBody] ForgotPasswordRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _authService.RequestPasswordResetOtpAsync(request.Email);
            return Ok(response);
        }

        [HttpPost("reset-password-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordWithOtp([FromBody] ResetPasswordWithOtpRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Service sẽ xử lý việc so sánh NewPassword và ConfirmPassword không cần thiết ở đây nếu DTO đã có Compare attribute
            var response = await _authService.ResetPasswordWithOtpAsync(request.OtpCode, request.NewPassword);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("hard-reset-password")]
        public async Task<IActionResult> HardResetPassword(int userId, [FromBody]HardResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _authService.HardResetPassword(userId, dto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
