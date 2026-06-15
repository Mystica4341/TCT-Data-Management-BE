using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Application.Service;
using sapo_clone.Domain.DTOs.WebRequest.ProductFactory;
using sapo_clone.Domain.DTOs.WebRequest.ProductSize;

namespace sapo_clone.API.Controllers
{
    [Route("api/factories")]
    [ApiController]
    public class ProductFactorysController : ControllerBase
    {
        private readonly IProductFactoryService _productFactoryService;
        public ProductFactorysController(IProductFactoryService productFactoryService)
        {
            _productFactoryService = productFactoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFactories(string? term = "")
        {
            return Ok(await _productFactoryService.ListAllFactories(term));
        }

        [HttpGet("supplier")]
        public async Task<IActionResult> GetAllFactories(int supplierId)
        {
            return Ok(await _productFactoryService.GetFactoriesBySupplier(supplierId));
        }

        [HttpPost("add-factory")]
        public async Task<IActionResult> AddNewFactory([FromBody] AddProductFactoryDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _productFactoryService.AddNewFactory(dto, cancellationToken));
            }
        }

        [HttpPut("update-factory")]
        public async Task<IActionResult> UpdateFactory(int factoryId, [FromBody] UpdateProductFactoryDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (factoryId <= 0)
            {
                return BadRequest("Không tìm thấy kích thước");
            }
            else
            {
                return Ok(await _productFactoryService.UpdateFactory(factoryId, dto, cancellationToken));
            }
        }

        [HttpDelete("delete-factory")]
        public async Task<IActionResult> DeleteFactory(int factoryId, CancellationToken cancellationToken)
        {
            if (factoryId <= 0)
            {
                return BadRequest("Không tìm thấy nhà máy");
            }
            else
            {
                return Ok(await _productFactoryService.DeleteFactory(factoryId, cancellationToken));
            }
        }

    }
}
