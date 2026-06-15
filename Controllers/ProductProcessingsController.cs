using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Application.Service;
using sapo_clone.Domain.DTOs.WebRequest.ProductProcessing;
using sapo_clone.Domain.DTOs.WebRequest.ProductSize;

namespace sapo_clone.API.Controllers
{
    [Route("api/processings")]
    [ApiController]
    public class ProductProcessingsController : ControllerBase
    {
        private readonly IProductProcessingService _productProcessingService;
        public ProductProcessingsController(IProductProcessingService productProcessingService)
        {
            _productProcessingService = productProcessingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProcessings(string? term = "")
        {
            return Ok(await _productProcessingService.ListAllProcessings(term));
        }

        [HttpPost("add-processing")]
        public async Task<IActionResult> AddNewProcessing([FromBody] AddProductProcessingDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _productProcessingService.AddNewProcessing(dto, cancellationToken));
            }
        }

        [HttpPut("update-processing")]
        public async Task<IActionResult> UpdateProcessing(int processingId, [FromBody] UpdateProductProcessingDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (processingId <= 0)
            {
                return BadRequest("Không tìm thấy gia công");
            }
            else
            {
                return Ok(await _productProcessingService.UpdateProcessing(processingId, dto, cancellationToken));
            }
        }

        [HttpDelete("delete-processing")]
        public async Task<IActionResult> DeleteProcessing(int processingId, CancellationToken cancellationToken)
        {
            if (processingId <= 0)
            {
                return BadRequest("Không tìm thấy gia công");
            }
            else
            {
                return Ok(await _productProcessingService.DeleteProcessing(processingId, cancellationToken));
            }
        }
    } 
}
