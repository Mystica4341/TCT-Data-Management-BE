using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Domain.DTOs.WebRequest.Setting;

namespace sapo_clone.API.Controllers
{
    [Route("api/settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _settingService;
        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetSetting()
        {
            return Ok(await _settingService.GetSetting());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-setting")]
        public async Task<IActionResult> UpdateSetting(int settingId, UpdateSettingDto dto, CancellationToken cancellationToken)
        {
            if (settingId <= 0)
            {
                return BadRequest("Không tìm thấy Id của thiết lập.");
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest("Sai định dạng truyền về. Vui lòng kiểm tra lại các thuộc tính.");
            }
            else
            {
                var response = await _settingService.UpdateSetting(settingId, dto, cancellationToken);
                return Ok(response);
            }
        }
    }
}
