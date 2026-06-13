using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Application.Service;
using sapo_clone.Domain.DTOs.WebRequest.ProductPattern;
using sapo_clone.Domain.DTOs.WebRequest.ProductSize;

namespace sapo_clone.API.Controllers
{
    [Route("api/sizes")]
    [ApiController]
    public class ProductSizesController : ControllerBase
    {
        private readonly IProductSizeService _productSizeService;
        public ProductSizesController(IProductSizeService productSizeService)
        {
            _productSizeService = productSizeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSizes(string? term = "")
        {
            return Ok(await _productSizeService.ListAllSizes(term));
        }

        [HttpPost("add-size")]
        public async Task<IActionResult> AddNewSize([FromBody] AddProductSizeDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _productSizeService.AddNewSize(dto, cancellationToken));
            }
        }

        [HttpPut("update-size")]
        public async Task<IActionResult> UpdateSize(int sizeId, [FromBody] UpdateProductSizeDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (sizeId <= 0)
            {
                return BadRequest("Không tìm thấy kích thước");
            }
            else
            {
                return Ok(await _productSizeService.UpdateSize(sizeId, dto, cancellationToken));
            }
        }

        [HttpDelete("delete-size")]
        public async Task<IActionResult> DeleteSize(int sizeId, CancellationToken cancellationToken)
        {
            if (sizeId <= 0)
            {
                return BadRequest("Không tìm thấy kích thước");
            }
            else
            {
                return Ok(await _productSizeService.DeleteSize(sizeId, cancellationToken));
            }
        }
    }
}
