using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Application.Service;
using sapo_clone.Domain.DTOs.WebRequest.ProductStorage;
using sapo_clone.Domain.DTOs.WebRequest.ProductSurface;

namespace sapo_clone.API.Controllers
{
    [Route("api/storages")]
    [ApiController]
    public class ProductStoragesController : ControllerBase
    {
        private readonly IProductStorageService _productStorageService;
        public ProductStoragesController(IProductStorageService productStorageService)
        {
            _productStorageService = productStorageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStorages(string? term = "")
        {
            return Ok(await _productStorageService.ListAllStorages(term));
        }

        [HttpPost("add-storage")]
        public async Task<IActionResult> AddNewStorage([FromBody] AddProductStorageDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _productStorageService.AddNewStorage(dto, cancellationToken));
            }
        }

        [HttpPut("update-storage")]
        public async Task<IActionResult> UpdateStorage(int storageId, [FromBody] UpdateProductStorageDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (storageId <= 0)
            {
                return BadRequest("Không tìm thấy kho");
            }
            else
            {
                return Ok(await _productStorageService.UpdateStorage(storageId, dto, cancellationToken));
            }
        }

        [HttpDelete("delete-storage")]
        public async Task<IActionResult> DeleteStorage(int storageId, CancellationToken cancellationToken)
        {
            if (storageId <= 0)
            {
                return BadRequest("Không tìm thấy kho");
            }
            else
            {
                return Ok(await _productStorageService.DeleteStorage(storageId, cancellationToken));
            }
        }
    } 
}
