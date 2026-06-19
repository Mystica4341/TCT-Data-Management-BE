using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sapo_clone.Application.IService;
using sapo_clone.Domain.DTOs.Common;
using sapo_clone.Domain.DTOs.Common.ResponseModel;
using sapo_clone.Domain.DTOs.WebRequest.Product;
using sapo_clone.Domain.DTOs.WebRequest.Product.Strategist;
using sapo_clone.Domain.DTOs.WebResponse.Product;
using sapo_clone.Domain.Entities;
using sapo_clone.Domain.Enum.Product;
using System.ComponentModel;
using System.Drawing;
using System.Security.Claims;

namespace sapo_clone.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] AdvancedSearchDto? filter)
        {
            try
            {
                var response = await _productService.ListProducts(filter);
                var result = new ApiResponseModel<PagedModel<ProductDto>>();
                if (response == null)
                {
                    result.Errors.Add("Không có sản phẩm nào!");
                    return BadRequest(result);
                }
                else
                {
                    result.Data = response;
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine("Error in GetProducts: " + ex.ToString());
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet("product-metadata")]
        public async Task<IActionResult> GetProductCategoryMetadata(CancellationToken cancellationToken)
        {
            return Ok(await _productService.GetProductMetadata(cancellationToken));
        }

        [HttpGet("total-products")]
        public async Task<IActionResult> GetTotalProductCount()
        {
            return Ok(await _productService.GetTotalProductCount());
        }

        [HttpGet("total-priced-products")]
        public async Task<IActionResult> GetTotalPricedProductCount()
        {
            return Ok(await _productService.GetTotalPricedProductCount());
        }

        [Authorize(Roles = "Admin,Strategist,SalesHead,PurchasingHead,MarketingHead")]
        [HttpGet("strategy-products")]
        public async Task<IActionResult> GetProductsStrategy([FromQuery] AdvancedSearchDto? filter)
        {
            var response = await _productService.ListProductsStrategy(filter);
            var result = new ApiResponseModel<PagedModel<StrategyProductDto>>();
            if (response == null)
            {
                result.Errors.Add("Không có sản phẩm nào!");
                return BadRequest(result);
            }
            else
            {
                result.Data = response;
                return Ok(result);
            }
        }

        [HttpGet("product-detail")]
        public async Task<IActionResult> GetProductDetail(int productId)
        {
            if (productId == 0)
            {
                return BadRequest("Không tìm thấy sản phẩm với Id = 0.");
            }
            else
            {
                var product = await _productService.GetProductDetail(productId);
                return Ok(product);
            }
        }

        [Authorize(Roles = "Admin,Strategist,SalesHead,PurchasingHead,MarketingHead")]
        [HttpGet("strategy-product-detail")]
        public async Task<IActionResult> GetStrategyProductDetail(int productId)
        {
            if (productId == 0)
            {
                return BadRequest("Không tìm thấy sản phẩm với Id = 0.");
            }
            else
            {
                var product = await _productService.GetStrategyProductDetail(productId);
                return Ok(product);
            }
        }

        [HttpGet("suppliers-order")]
        public async Task<IActionResult> GetOrderNumberFromSupplier(int supplierId)
        {
            if (supplierId == 0)
            {
                return BadRequest("Không thấy nhà cung cấp này!");
            }
            else
            {
                var productCount = await _productService.GetOrderNumberFromSupplier(supplierId);
                return Ok(productCount);
            }
        }

        [HttpGet("check-supplier-item-code")]
        public async Task<IActionResult> CheckSupplierItemCode(string supplierItemCode)
        {
            if (supplierItemCode == null)
            {
                return BadRequest("Mã số nhà cung cấp.");
            }
            else
            {
                return Ok(await _productService.IsSupplierItemCodeExist(supplierItemCode));
            }
        }

        [HttpGet("existing-supplier-size-combinations")]
        public async Task<IActionResult> GetExistingSupplierSizeCombinations()
        {
            return Ok(await _productService.GetExistingSupplierSizeCombinationsAsync());
        }

        [HttpGet("generate-report")]
        public async Task<IActionResult> GenerateReport(CancellationToken cancellationToken)
        {
            return Ok(await _productService.GenerateProductReportAsync(cancellationToken));
        }

        [HttpGet("get-statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] UploadWebsiteStatus? status)
        {
            return Ok(await _productService.GetStatistics(startDate, endDate, status));
        }

        //[HttpPut("update-date")]
        //public async Task<IActionResult> UpdateDate([FromBody] List<int> productIds)
        //{
        //    await _productService.UpdateProductCreatedDate(productIds);
        //    return Ok("Cập nhật thành công ngày");
        //}

        [HttpPost("calculate-processing-product")]
        public async Task<IActionResult> CalculateProcessingProduct([FromBody] CalculateProcessingPriceRequest dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Yêu cầu không hợp lệ");
            }
            else
            {
                return Ok(await _productService.CalculatePriceForProcessedProductAsync(dto, cancellationToken));
            }
        }

        [Authorize]
        [HttpPost("add-new")]
        public async Task<IActionResult> AddNewProduct(AddProductDto dto, CancellationToken cancellationToken)
        {
            if (dto == null)
            {
                return BadRequest("Vui lòng nhập đầy đủ thông tin.");
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest("Vui lòng kiểm tra các thông tin đã đúng định dạng hay chưa.");
            }
            else
            {
                var product = await _productService.AddProduct(dto, User.FindFirstValue(ClaimTypes.Email), cancellationToken);
                return Ok(product);
            }
        }

        [Authorize]
        [HttpPost("import")]
        public async Task<IActionResult> ImportExcel(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null)
            {
                return BadRequest("Không tìm thấy File.");
            }

            var product = await _productService.ImportProductsFromExcelAsync(file, User.FindFirstValue(ClaimTypes.Email), cancellationToken);
            return Ok(product);
        }

        [HttpPost("export")] // Thường dùng POST nếu AdvancedSearchDto có nhiều param phức tạp
        public async Task<IActionResult> ExportProductsToExcel([FromBody] AdvancedSearchDto filter, CancellationToken cancellationToken)
        {
            // Tương tự như hình của bạn, gọi thẳng Service
            var fileBytes = await _productService.ExportProductsToExcelAsync(filter, cancellationToken);

            // Nếu không có dữ liệu hoặc lỗi (tùy bạn handle logic ở Service)
            if (fileBytes == null || fileBytes.Length == 0)
            {
                return BadRequest("Không thể xuất file dữ liệu.");
            }

            // Định dạng trả về file Excel
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = $"DanhSachSanPham_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            // Trả về file cho Frontend tải xuống
            return File(fileBytes, contentType, fileName);
        }

        [Authorize]
        [HttpPut("edit-product")]
        public async Task<IActionResult> EditProduct(int productId, [FromBody] EditProductDto dto, CancellationToken cancellationToken)
        {
            if (productId <= 0)
            {
                return BadRequest("Không tìm thấy mã sản phẩm.");
            }
            else if (dto == null)
            {
                return BadRequest("Vui lòng nhập đầy đủ thông tin.");
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest("Vui lòng kiểm tra các thông tin đã đúng định dạng hay chưa.");
            }
            else
            {
                var product = await _productService.EditProduct(productId, dto, User.FindFirstValue(ClaimTypes.Email), cancellationToken);
                return Ok(product);
            }
        }

        [Authorize]
        [HttpPut("update-batch-product")]
        public async Task<IActionResult> UpdateBatchProducts(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null)
            {
                return BadRequest("Không tìm thấy File.");
            }

            var product = await _productService.UpdateBatchFromExcel(file, User.FindFirstValue(ClaimTypes.Email), cancellationToken);
            return Ok(product);
        }

        [Authorize(Roles = "Admin,Strategist,SalesHead,PurchasingHead,MarketingHead")]
        [HttpPut("strategy-edit-product")]
        public async Task<IActionResult> EditStrategyProduct(int productId, [FromBody] EditStrategyProductDto dto, CancellationToken cancellationToken)
        {
            if (productId <= 0)
            {
                return BadRequest("Không tìm thấy mã sản phẩm.");
            }
            else if (dto == null)
            {
                return BadRequest("Vui lòng nhập đầy đủ thông tin.");
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest("Vui lòng kiểm tra các thông tin đã đúng định dạng hay chưa.");
            }
            else
            {
                var product = await _productService.EditStrategyProduct(productId, dto, User.FindFirstValue(ClaimTypes.Email), cancellationToken);
                return Ok(product);
            }
        }

        [Authorize(Roles = "Admin,Strategist,SalesHead,PurchasingHead,MarketingHead")]
        [HttpPut("bulk-strategy-edit-product")]
        public async Task<IActionResult> BulkEditStrategyProduct([FromBody] BulkStrategyProductUpdateDto dto, CancellationToken cancellationToken)
        {
            if (dto == null)
            {
                return BadRequest("Vui lòng nhập đầy đủ thông tin.");
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest("Vui lòng kiểm tra các thông tin đã đúng định dạng hay chưa.");
            }
            else
            {
                var product = await _productService.BulkEditStrategyProducts(dto, User.FindFirstValue(ClaimTypes.Email), cancellationToken);
                return Ok(product);
            }
        }

        [HttpPut("update-color")]
        public async Task<IActionResult> UpdateColor(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null)
            {
                return BadRequest("Không tìm thấy File.");
            }

            var product = await _productService.UpdateProductColorBatchFromExcelAsync(file, User.FindFirstValue(ClaimTypes.Email), cancellationToken);
            return Ok(product);
        }

        [HttpDelete("delete-product")]
        public async Task<IActionResult> DeleteProduct(int productId, CancellationToken cancellationToken)
        {
            if (productId <= 0)
            {
                return BadRequest("Không tìm thấy mã sản phẩm.");
            }
            else
            {
                var result = await _productService.DeleteProduct(productId, cancellationToken);
                return Ok(result);
            }
        }
    }
}
