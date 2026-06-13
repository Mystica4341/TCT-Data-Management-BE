using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sapo_clone.Application.IService;
using sapo_clone.Application.Service;
using sapo_clone.Domain.DTOs.WebRequest.ProductSupplier;
using sapo_clone.Domain.DTOs.WebRequest.ProductSurface;
using System.Security.Claims;

namespace sapo_clone.API.Controllers
{
    [Route("api/suppliers")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class ProductSuppliersController : ControllerBase
    {
        private readonly IProductSupplierService _productSupplierService;
        public ProductSuppliersController(IProductSupplierService productSupplierService)
        {
            _productSupplierService = productSupplierService;
        }


        [HttpGet("get-order")]
        public async Task<IActionResult> GetOrderNumber(string term)
        {
            if (term.IsNullOrEmpty())
            {
                return BadRequest("Không tìm thấy giá trị mã");
            }
            else
            {
                var count = await _productSupplierService.GetOrderOfSupplier(term, CancellationToken.None);
                return Ok(count);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers(string? term = "")
        {
            return Ok(await _productSupplierService.ListAllSuppliers(term));
        }

        [Authorize(Roles = "Admin,Strategist,SalesHead,PurchasingHead,MarketingHead")]
        [HttpGet("supplier-detail")]
        public async Task<IActionResult> GetSupplierDetailForStrategist(int supplierId)
        {
            return Ok(await _productSupplierService.GetSupplierDetail(supplierId));
        }

        [Authorize]
        [HttpPost("add-supplier")]
        public async Task<IActionResult> AddNewSupplier([FromBody] AddProductSupplierDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _productSupplierService.AddNewSupplier(dto, cancellationToken));
            }
        }

        [Authorize]
        [HttpPut("update-supplier")]
        public async Task<IActionResult> UpdateSupplier(int supplierId, [FromBody] UpdateProductSupplierDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (supplierId <= 0)
            {
                return BadRequest("Không tìm thấy nhà cung cấp");
            }
            else
            {
                return Ok(await _productSupplierService.UpdateSupplier(supplierId, dto, cancellationToken));
            }
        }

        [Authorize(Roles = "Admin,Strategist,SalesHead,PurchasingHead,MarketingHead")]
        [HttpPut("strategist-update-supplier")]
        public async Task<IActionResult> UpdateSupplierForStrategist(int supplierId, [FromBody] UpdateProductSupplierDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (supplierId <= 0)
            {
                return BadRequest("Không tìm thấy nhà cung cấp");
            }
            else
            {
                return Ok(await _productSupplierService.UpdateSupplierForStrategist(supplierId, dto, User.FindFirstValue(ClaimTypes.Email), cancellationToken));
            }
        }

    }
}
