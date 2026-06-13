using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Application.Service;
using sapo_clone.Domain.DTOs.WebRequest.ProductMaterial;
using sapo_clone.Domain.DTOs.WebRequest.ProductSize;

namespace sapo_clone.API.Controllers
{
    [Route("api/materials")]
    [ApiController]
    public class ProductMaterialsController : ControllerBase
    {
        private readonly IProductMaterialService _productMaterialService;
        public ProductMaterialsController(IProductMaterialService productMaterialService)
        {
            _productMaterialService = productMaterialService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMaterials(string? term = "")
        {
            return Ok(await _productMaterialService.ListAllMaterials(term));
        }

        [HttpPost("add-material")]
        public async Task<IActionResult> AddNewMaterial([FromBody] AddProductMaterialDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _productMaterialService.AddNewMaterial(dto, cancellationToken));
            }
        }

        [HttpPut("update-material")]
        public async Task<IActionResult> UpdateMaterial(int materialId, [FromBody] UpdateProductMaterialDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (materialId <= 0)
            {
                return BadRequest("Không tìm thấy chất liệu");
            }
            else
            {
                return Ok(await _productMaterialService.UpdateMaterial(materialId, dto, cancellationToken));
            }
        }
    }
}
