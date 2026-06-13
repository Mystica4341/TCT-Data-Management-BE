using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Domain.DTOs.WebRequest.Policy;

namespace sapo_clone.API.Controllers
{
    [Route("api/policies")]
    [ApiController]
    [Authorize] // Yêu cầu phải đăng nhập (có Token) mới được gọi các API này
    public class PoliciesController : ControllerBase
    {
        private readonly IPolicyService _policyService;

        public PoliciesController(IPolicyService policyService)
        {
            _policyService = policyService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value
                ?? User.FindFirst("id")?.Value;

            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        /// <summary>
        /// Lấy danh sách tất cả chính sách (có phân trang/tìm kiếm nếu cần)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? term, CancellationToken cancellationToken)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
            {
                return Unauthorized(new { Message = "Khong xac dinh duoc danh tinh nguoi dung." });
            }

            var response = await _policyService.ListAllPolicies(currentUserId, term, cancellationToken);

            if (!response.Success)
                return BadRequest(response); // Trả về 400 cùng danh sách lỗi

            return Ok(response); // Trả về 200 OK cùng dữ liệu
        }

        /// <summary>
        /// Lấy chi tiết một bài viết theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
            {
                return Unauthorized(new { Message = "Khong xac dinh duoc danh tinh nguoi dung." });
            }

            var response = await _policyService.GetPolicyDetail(id, currentUserId, cancellationToken);

            if (!response.Success)
                return NotFound(response); // Trả về 404 nếu không tìm thấy bài viết

            return Ok(response);
        }

        /// <summary>
        /// Lấy danh sách chính sách theo phòng ban
        /// </summary>
        [HttpGet("department/{department}")]
        public async Task<IActionResult> GetByDepartment(string department, CancellationToken cancellationToken)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
            {
                return Unauthorized(new { Message = "Khong xac dinh duoc danh tinh nguoi dung." });
            }

            var response = await _policyService.GetPoliciesByDepartment(currentUserId, department, cancellationToken);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Thêm mới chính sách (Chỉ Admin/Manager mới có quyền)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,SystemHandler,MarketingHead,SalesHead,PurchasingHead,StorageHead,PmhHead,WarehouseDeliveryHead")]
        public async Task<IActionResult> Create([FromBody] CreatePolicyDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
            {
                return Unauthorized(new { Message = "Khong xac dinh duoc danh tinh nguoi dung." });
            }

            var response = await _policyService.AddNewPolicy(currentUserId, dto, cancellationToken);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Cập nhật nội dung chính sách (Chỉ Admin/Manager mới có quyền)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SystemHandler,MarketingHead,SalesHead,PurchasingHead,StorageHead,PmhHead,WarehouseDeliveryHead")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePolicyDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
            {
                return Unauthorized(new { Message = "Khong xac dinh duoc danh tinh nguoi dung." });
            }

            var response = await _policyService.UpdatePolicy(id, currentUserId, dto, cancellationToken);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
