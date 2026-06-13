using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IRepository;
using sapo_clone.Application.IService;
using sapo_clone.Domain.DTOs.WebRequest.ProductBodyColor;
using sapo_clone.Domain.DTOs.WebRequest.ProductColor;

namespace sapo_clone.API.Controllers
{
    [Route("api/body-colors")]
    [ApiController]
    public class ProductBodyColorsController : ControllerBase
    {
        private readonly IProductBodyColorService _productBodyColorService;
        public ProductBodyColorsController(IProductBodyColorService productBodyColorService)
        {
            _productBodyColorService = productBodyColorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBodyColors(string? term = "")
        {
            return Ok(await _productBodyColorService.ListAllBodyColors(term));
        }

        [HttpPost("add-body-color")]
        public async Task<IActionResult> AddNewBodyColor([FromBody] AddProductBodyColorDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _productBodyColorService.AddNewBodyColor(dto, cancellationToken));
            }
        }

        [HttpPut("update-body-color")]
        public async Task<IActionResult> UpdateBodyColor(int bodyColorId, [FromBody] UpdateProductBodyColorDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (bodyColorId <= 0)
            {
                return BadRequest("Không tìm thấy màu sắc thân gạch");
            }
            else
            {
                return Ok(await _productBodyColorService.UpdateBodyColor(bodyColorId, dto, cancellationToken));
            }
        }
    }
}
