using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Domain.DTOs.WebRequest.CaseStudy;
using System.Security.Claims;

namespace sapo_clone.API.Controllers
{
    [Route("api/case-studies")]
    [ApiController]
    [Authorize] // Bắt buộc đăng nhập
    public class CaseStudiesController : ControllerBase
    {
        // GIẢ ĐỊNH: Bạn đã tạo ICaseStudyService và ICaseStudyLikeService 
        // để gọi xuống Repository ở các bước trước.
        private readonly ICaseStudyService _caseStudyService;
        private readonly ICaseStudyLikeService _caseStudyLikeService;

        public CaseStudiesController(
            ICaseStudyService caseStudyService,
            ICaseStudyLikeService caseStudyLikeService)
        {
            _caseStudyService = caseStudyService;
            _caseStudyLikeService = caseStudyLikeService;
        }

        // --- HELPER: LẤY ID NGƯỜI DÙNG TỪ TOKEN JWT ---
        private int GetCurrentUserId()
        {
            // Tùy thuộc vào cấu hình JWT của bạn, claim chứa ID thường là NameIdentifier hoặc Name
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value
                           ?? User.FindFirst("id")?.Value;

            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return 0; // Hoặc ném ra Exception Unauthorized
        }

        /// <summary>
        /// Lấy danh sách bài chia sẻ (Lọc theo COMMON / SPECIFIC)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? term, [FromQuery] string? caseType, CancellationToken cancellationToken)
        {
            var response = await _caseStudyService.GetCaseStudiesAsync(term, caseType, cancellationToken);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Lấy chi tiết bài viết (Kèm trạng thái User hiện tại đã Like chưa)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            int currentUserId = GetCurrentUserId();
            var response = await _caseStudyService.GetCaseStudyDetailAsync(id, currentUserId, cancellationToken);

            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        /// <summary>
        /// Thêm bài viết mới
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,SystemHandler,MarketingHead,SalesHead,PurchasingHead,StorageHead,PmhHead,WarehouseDeliveryHead")]
        public async Task<IActionResult> Create([FromBody] CreateCaseStudyDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            int currentUserId = GetCurrentUserId();
            if (currentUserId == 0) return Unauthorized(new { Message = "Khong xac dinh duoc danh tinh nguoi dung." });
            var response = await _caseStudyService.CreateCaseStudyAsync(currentUserId, dto, cancellationToken);

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Sửa bài viết
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SystemHandler,MarketingHead,SalesHead,PurchasingHead,StorageHead,PmhHead,WarehouseDeliveryHead")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCaseStudyDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            int currentUserId = GetCurrentUserId();
            if (currentUserId == 0) return Unauthorized(new { Message = "Khong xac dinh duoc danh tinh nguoi dung." });
            var response = await _caseStudyService.UpdateCaseStudyAsync(id, currentUserId, dto, cancellationToken);

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Xóa bài viết
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SystemHandler,MarketingHead,SalesHead,PurchasingHead,StorageHead,PmhHead,WarehouseDeliveryHead")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            int currentUserId = GetCurrentUserId();
            if (currentUserId == 0) return Unauthorized(new { Message = "Khong xac dinh duoc danh tinh nguoi dung." });
            var response = await _caseStudyService.DeleteCaseStudyAsync(id, currentUserId, cancellationToken);

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// [TÍNH NĂNG ĐẶC BIỆT] - Thả tim / Rút tim
        /// </summary>
        [HttpPost("{id}/like")]
        public async Task<IActionResult> ToggleLike(int id, CancellationToken cancellationToken)
        {
            int currentUserId = GetCurrentUserId();
            if (currentUserId == 0) return Unauthorized(new { Message = "Không xác định được danh tính người dùng." });

            var response = await _caseStudyLikeService.ToggleLikeAsync(id, currentUserId, cancellationToken);

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// [TÍNH NĂNG ADMIN] - Bật/Tắt tích xanh xác thực
        /// </summary>
        [HttpPatch("{id}/verify")]
        // [Authorize(Roles = "Admin,Manager")] // Chỉ định rõ quyền
        public async Task<IActionResult> ToggleVerify(int id, CancellationToken cancellationToken)
        {
            var response = await _caseStudyService.ToggleAdminVerificationAsync(id, cancellationToken);

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
    }
}
