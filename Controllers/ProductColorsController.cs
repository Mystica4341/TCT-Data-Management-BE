using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Application.Service;
using sapo_clone.Domain.DTOs.WebRequest.ProductColor;
using sapo_clone.Domain.DTOs.WebRequest.ProductMaterial;

namespace sapo_clone.API.Controllers
{
    [Route("api/colors")]
    [ApiController]
    public class ProductColorsController : ControllerBase
    {
        private readonly IProductColorService _productColorService;
        public ProductColorsController(IProductColorService productColorService)
        {
            _productColorService = productColorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlColors(string? term = "")
        {
            return Ok(await _productColorService.ListAllColors(term));
        }

        [HttpPost("add-color")]
        public async Task<IActionResult> AddNewColor([FromBody] AddProductColorDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _productColorService.AddNewColor(dto, cancellationToken));
            }
        }

        [HttpPut("update-color")]
        public async Task<IActionResult> UpdateColor(int colorId, [FromBody] UpdateProductColorDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (colorId <= 0)
            {
                return BadRequest("Không tìm thấy màu sắc");
            }
            else
            {
                return Ok(await _productColorService.UpdateColor(colorId, dto, cancellationToken));
            }
        }

        [HttpDelete("delete-color")]
        public async Task<IActionResult> DeleteColor(int colorId, CancellationToken cancellationToken)
        {
            if (colorId <= 0)
            {
                return BadRequest("Không tìm thấy màu sắc");
            }
            else
            {
                return Ok(await _productColorService.DeleteColor(colorId, cancellationToken));
            }
        }
    }
}
