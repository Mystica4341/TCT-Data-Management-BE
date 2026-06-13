using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Domain.DTOs.WebRequest.PolicyCategory;

namespace sapo_clone.API.Controllers
{
    [Route("api/policy-categories")]
    [ApiController]
    [Authorize] // Bắt buộc đăng nhập
    public class PolicyCategoriesController : ControllerBase
    {
        private readonly IPolicyCategoryService _categoryService;

        // Tiêm Dependency (Dependency Injection) của Service vào Controller
        public PolicyCategoriesController(IPolicyCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Lấy danh sách tất cả danh mục (có thể tìm kiếm)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? term, CancellationToken cancellationToken)
        {
            var response = await _categoryService.GetAllCategories(term, cancellationToken);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Lấy chi tiết một danh mục (để đưa dữ liệu lên Form sửa)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var response = await _categoryService.GetCategoryById(id, cancellationToken);

            if (!response.Success)
                return NotFound(response); // Trả về 404 nếu không tồn tại

            return Ok(response);
        }

        /// <summary>
        /// Thêm mới một danh mục
        /// </summary>
        [HttpPost]
         [Authorize(Roles = "Admin")] // Mở khóa dòng này để phân quyền
        public async Task<IActionResult> Create([FromBody] CreatePolicyCategoryDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _categoryService.CreateCategory(dto, cancellationToken);

            if (!response.Success)
                return BadRequest(response); // Có thể do trùng tên danh mục

            return Ok(response);
        }

        /// <summary>
        /// Cập nhật thông tin danh mục
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Mở khóa dòng này để phân quyền
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePolicyCategoryDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _categoryService.UpdateCategory(id, dto, cancellationToken);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Xóa danh mục
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Mở khóa dòng này để phân quyền
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var response = await _categoryService.DeleteCategory(id, cancellationToken);

            // Chỗ này rất quan trọng: Nếu danh mục đang có Policy bên trong, 
            // EF Core sẽ ném Exception, Repository bắt lại đưa vào response.Errors 
            // và Controller sẽ trả về 400 kèm câu thông báo.
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
