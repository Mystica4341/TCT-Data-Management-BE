using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Application.Service;
using sapo_clone.Domain.DTOs.WebRequest.ProductSurface;
using sapo_clone.Domain.DTOs.WebRequest.WaterAbsorption;

namespace sapo_clone.API.Controllers
{
    [Route("api/water-absorptions")]
    [ApiController]
    public class WaterAbsorptionsController : ControllerBase
    {
        private readonly IProductWaterAbsorptionService _waterAbsorptionService;
        public WaterAbsorptionsController(IProductWaterAbsorptionService waterAbsorptionService)
        {
            _waterAbsorptionService = waterAbsorptionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWaterAbsorptions(string? term = "")
        {
            return Ok(await _waterAbsorptionService.ListAllWaterAbsorption(term));
        }

        [HttpPost("add-water-absorption")]
        public async Task<IActionResult> AddNewWaterAbsorption([FromBody] AddWaterAbsorptionDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _waterAbsorptionService.AddNewWaterAbsorption(dto, cancellationToken));
            }
        }

        [HttpPut("update-water-absorption")]
        public async Task<IActionResult> UpdateWaterAbsorption(int waterAbsorptionId, [FromBody] UpdateWaterAbsorptionDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (waterAbsorptionId <= 0)
            {
                return BadRequest("Không tìm thấy độ hút nước");
            }
            else
            {
                return Ok(await _waterAbsorptionService.UpdateWaterAbsorption(waterAbsorptionId, dto, cancellationToken));
            }
        }
    }
}
