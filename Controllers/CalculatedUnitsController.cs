using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Application.IService;
using sapo_clone.Application.Service;
using sapo_clone.Domain.DTOs.WebRequest.CalculatedUnit;
using sapo_clone.Domain.DTOs.WebRequest.CompanyCode;

namespace sapo_clone.API.Controllers
{
    [Route("api/calculated-units")]
    [ApiController]
    public class CalculatedUnitsController : ControllerBase
    {
        private readonly ICalculatedUnitService _calculatedUnitService;
        public CalculatedUnitsController(ICalculatedUnitService calculatedUnitService)
        {
            _calculatedUnitService = calculatedUnitService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCalculatedUnits(string? term = "")
        {
            return Ok(await _calculatedUnitService.ListAllCalculatedUnit(term));
        }

        [HttpPost("add-calculated-unit")]
        public async Task<IActionResult> AddNewCalculatedUnit([FromBody] AddCalculatedUnitDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _calculatedUnitService.AddNewCalculatedUnit(dto, cancellationToken));
            }
        }

        [HttpPut("update-calculated-unit")]
        public async Task<IActionResult> UpdateCalculatedUnit(int calId, [FromBody] UpdateCalculatedUnitDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (calId <= 0)
            {
                return BadRequest("Không tìm thấy đơn vị tính");
            }
            else
            {
                return Ok(await _calculatedUnitService.UpdateCalculatedUnit(calId, dto, cancellationToken));
            }
        }

        [HttpDelete("delete-calculated-unit")]
        public async Task<IActionResult> DeleteCalculatedUnit(int calId, CancellationToken cancellationToken)
        {
            if (calId <= 0)
            {
                return BadRequest("Không tìm thấy đơn vị tính");
            }
            else
            {
                return Ok(await _calculatedUnitService.DeleteCalculatedUnit(calId, cancellationToken));
            }
        }
    }
}
