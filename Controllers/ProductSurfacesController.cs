using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Application.Service;
using sapo_clone.Domain.DTOs.WebRequest.ProductMaterial;
using sapo_clone.Domain.DTOs.WebRequest.ProductSurface;

namespace sapo_clone.API.Controllers
{
    [Route("api/surfaces")]
    [ApiController]
    public class ProductSurfacesController : ControllerBase
    {
        private readonly IProductSurfaceService _productSurfaceService;
        public ProductSurfacesController(IProductSurfaceService productSurfaceService)
        {
            _productSurfaceService = productSurfaceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSurfaces(string? term = "")
        {
            return Ok(await _productSurfaceService.ListAllSurfaces(term));
        }

        [HttpPost("add-surface")]
        public async Task<IActionResult> AddNewSurface([FromBody] AddProductSurfaceDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _productSurfaceService.AddNewSurface(dto, cancellationToken));
            }
        }

        [HttpPut("update-surface")]
        public async Task<IActionResult> UpdateSurface(int surfaceId, [FromBody] UpdateProductSurfaceDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (surfaceId <= 0)
            {
                return BadRequest("Không tìm thấy bề mặt");
            }
            else
            {
                return Ok(await _productSurfaceService.UpdateSurface(surfaceId, dto, cancellationToken));
            }
        }
    }
}
