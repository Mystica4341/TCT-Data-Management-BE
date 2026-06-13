using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Domain.DTOs.WebRequest.User;

namespace sapo_clone.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(string? term = "")
        {
            return Ok(await _userService.GetAllUsers(term));
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddNewUser([FromBody] AddUserDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var result = await _userService.AddUser(dto, cancellationToken);
                return Ok(result);
            }
        }

        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser(string userEmail, [FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _userService.UpdateUser(userEmail, dto, cancellationToken));
            }
        }

        [HttpPut("ban-user")]
        public async Task<IActionResult> BanUser(string userEmail, CancellationToken cancellationToken)
        {
            var result = await _userService.BanUser(userEmail, cancellationToken);
            return Ok(result);
        }

        [HttpPut("unban-user")]
        public async Task<IActionResult> UnbanUser(string userEmail, CancellationToken cancellationToken)
        {
            var result = await _userService.UnbanUser(userEmail, cancellationToken);
            return Ok(result);
        }

        [HttpPut("update-avatar")]
        public async Task<IActionResult> UpdateAvatar(string userEmail, IFormFile file, CancellationToken cancellationToken)
        {
            var result = await _userService.UpdateAvatar(userEmail, file, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("delete-user")]
        public async Task<IActionResult> DeleteUser(string userEmail, CancellationToken cancellationToken)
        {
            var result = await _userService.DeleteUser(userEmail, cancellationToken);
            return Ok(result);
        }
    }
}
