using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IRepository;
using sapo_clone.Application.IService;
using sapo_clone.Domain.DTOs.WebRequest.ProductAntiSlippery;
using sapo_clone.Domain.DTOs.WebRequest.ProductSurface;

namespace sapo_clone.API.Controllers
{
    [Route("api/anti-slipperys")]
    [ApiController]
    public class ProductAntiSlipperysController : ControllerBase
    {
        private readonly IProductAntiSlipperyService _productAntiSlipperyService;
        public ProductAntiSlipperysController(IProductAntiSlipperyService productAntiSlipperyService)
        {
            _productAntiSlipperyService = productAntiSlipperyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAntiSlipperys(string? term = "")
        {
            return Ok(await _productAntiSlipperyService.ListAntiSlipperys(term));
        }

        [HttpPost("add-anti-slippery")]
        public async Task<IActionResult> AddNewAntiSlippery([FromBody] AddProductAntiSlipperyDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _productAntiSlipperyService.AddNewAntiSlippery(dto, cancellationToken));
            }
        }

        [HttpPut("update-anti-slippery")]
        public async Task<IActionResult> UpdateAntiSlippery(int antiSlipperyId, [FromBody] UpdateProductAntiSlipperyDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (antiSlipperyId <= 0)
            {
                return BadRequest("Không tìm thấy độ chống trượt");
            }
            else
            {
                return Ok(await _productAntiSlipperyService.UpdateAntiSlippery(antiSlipperyId, dto, cancellationToken));
            }
        }
    }
}
