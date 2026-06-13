using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Application.Service;
using sapo_clone.Domain.DTOs.WebRequest.ProductPattern;

namespace sapo_clone.API.Controllers
{
    [Route("api/patterns")]
    [ApiController]
    public class ProductPatternsController : ControllerBase
    {
        private readonly IProductPatternService _productPatternService;
        public ProductPatternsController(IProductPatternService productPatternService)
        {
            _productPatternService = productPatternService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatterns(string? term = "")
        {
            return Ok(await _productPatternService.ListAllPatterns(term));
        }

        [HttpPost("add-pattern")]
        public async Task<IActionResult> AddNewPattern([FromBody] AddPatternDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _productPatternService.AddNewPattern(dto, cancellationToken));
            }
        }

        [HttpPut("update-pattern")]
        public async Task<IActionResult> UpdatePattern(int patternId, [FromBody] UpdatePatternDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (patternId <= 0)
            {
                return BadRequest("Không tìm thấy hệ vân");
            }
            else
            {
                return Ok(await _productPatternService.UpdatePattern(patternId, dto, cancellationToken));
            }
        }
    }
}
