using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Application.Service;
using sapo_clone.Domain.DTOs.WebRequest.ProductArea;

namespace sapo_clone.API.Controllers
{
    [Route("api/areas")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly IProductAreaService _productAreaService;
        public AreasController(IProductAreaService productAreaService)
        {
            _productAreaService = productAreaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAreas(string? term = "")
        {
            return Ok(await _productAreaService.ListAllAreas(term));
        }

        [HttpPost("add-area")]
        public async Task<IActionResult> AddNewArea([FromBody] AddAreaDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _productAreaService.AddNewArea(dto, cancellationToken));
            }
        }

        [HttpPut("update-area")]
        public async Task<IActionResult> UpdateArea(int areaId, [FromBody] UpdateAreaDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (areaId <= 0)
            {
                return BadRequest("Không tìm thấy khu vực");
            }
            else
            {
                return Ok(await _productAreaService.UpdateArea(areaId, dto, cancellationToken));
            }
        }

        [HttpDelete("delete-area")]
        public async Task<IActionResult> DeleteArea(int areaId, CancellationToken cancellationToken)
        {
            if (areaId <= 0)
            {
                return BadRequest("Không tìm thấy khu vực");
            }
            else
            {
                return Ok(await _productAreaService.DeleteArea(areaId, cancellationToken));
            }
        }
    }
}
