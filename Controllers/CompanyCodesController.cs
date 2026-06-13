using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Application.Service;
using sapo_clone.Domain.DTOs.WebRequest.CompanyCode;

namespace sapo_clone.API.Controllers
{
    [Route("api/company-codes")]
    [ApiController]
    public class CompanyCodesController : ControllerBase
    {
        private readonly ICompanyCodeService _companyCodeService;
        public CompanyCodesController(ICompanyCodeService companyCodeService)
        {
            _companyCodeService = companyCodeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanyCodes(string? term = "")
        {
            return Ok(await _companyCodeService.ListAllCompanyCode(term));
        }

        [HttpPost("add-company-code")]
        public async Task<IActionResult> AddNewCompanyCode([FromBody] AddCompanyCodeDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _companyCodeService.AddNewCompanyCode(dto, cancellationToken));
            }
        }

        [HttpPut("update-company-code")]
        public async Task<IActionResult> UpdateCompanyCode(int cpnCodeId, [FromBody] UpdateCompanyCodeDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (cpnCodeId <= 0)
            {
                return BadRequest("Không tìm thấy mã công ty");
            }
            else
            {
                return Ok(await _companyCodeService.UpdateCompanyCode(cpnCodeId, dto, cancellationToken));
            }
        }

        [HttpDelete("delete-company-code")]
        public async Task<IActionResult> DeleteCompanyCode(int cpnCodeId, CancellationToken cancellationToken)
        {
            if (cpnCodeId <= 0)
            {
                return BadRequest("Không tìm thấy mã công ty");
            }
            else
            {
                return Ok(await _companyCodeService.DeleteCompanyCode(cpnCodeId, cancellationToken));
            }
        }
    }
}
