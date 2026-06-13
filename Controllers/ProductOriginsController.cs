using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Domain.DTOs.WebRequest.ProductOrigin;

namespace sapo_clone.API.Controllers
{
    [Route("api/origins")]
    [ApiController]
    public class ProductOriginsController : ControllerBase
    {
        private readonly IProductOriginService _productOriginService;

        public ProductOriginsController(IProductOriginService productOriginService)
        {
            _productOriginService = productOriginService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrigins(string? term = "")
        {
            return Ok(await _productOriginService.ListAllOrigin(term));
        }

        [HttpPost("add-origin")]
        public async Task<IActionResult> AddNewOrigin([FromBody] AddProductOriginDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _productOriginService.AddNewOrigin(dto, cancellationToken));
            }
        }

        [HttpPut("update-origin")]
        public async Task<IActionResult> UpdateOrigin(int originId, [FromBody] UpdateProductOriginDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (originId <= 0)
            {
                return BadRequest("Không tìm thấy mã xuất xứ");
            }
            else
            {
                return Ok(await _productOriginService.UpdateOrigin(originId, dto, cancellationToken));
            }
        }

        [HttpDelete("delete-origin")]
        public async Task<IActionResult> DeleteOrigin(int originId, CancellationToken cancellationToken)
        {
            if (originId <= 0)
            {
                return BadRequest("Không tìm thấy mã xuất xứ");
            }
            else
            {
                return Ok(await _productOriginService.DeleteOrigin(originId, cancellationToken));
            }
        }
    }
}
