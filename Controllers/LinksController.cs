using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sapo_clone.Infrastructure.Services.GoogleDrive;
using sapo_clone.Domain.DTOs.WebRequest.Link;

namespace sapo_clone.API.Controllers
{
    [Route("api/links")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class LinksController : ControllerBase
    {
        private readonly ILinkService _linkService;
        public LinksController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLinks(string? term = "")
        {
            return Ok(await _linkService.ListAllLinks(term));
        }

        [HttpPost("add-link")]
        public async Task<IActionResult> AddNewLink([FromBody] AddLinkDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _linkService.AddNewLink(dto, cancellationToken));
            }
        }

        [HttpPut("update-link")]
        public async Task<IActionResult> UpdateLink(int linkId, [FromBody] UpdateLinkDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (linkId <= 0)
            {
                return BadRequest("Không tìm thấy liên kết");
            }
            else
            {
                return Ok(await _linkService.UpdateLink(linkId, dto, cancellationToken));
            }
        }

        [HttpDelete("delete-link")]
        public async Task<IActionResult> DeleteLink(int linkId, CancellationToken cancellationToken)
        {
            if (linkId <= 0)
            {
                return BadRequest("Không tìm thấy liên kết");
            }
            return Ok(await _linkService.DeleteLink(linkId, cancellationToken));
        }
    }
}
