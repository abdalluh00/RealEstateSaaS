using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Features.PropertyMedia.Commands.DeleteMedia;
using RealEstate.Application.Features.PropertyMedia.Commands.SetCover;
using RealEstate.Application.Features.PropertyMedia.Commands.UploadMedia;
using RealEstate.Application.Features.PropertyMedia.Queries.GetPropertyMedia;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyMediaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertyMediaController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{propertyId:guid}")]
        public async Task<IActionResult> GetAll(Guid propertyId)
        {
            var result = await _mediator.Send(new GetPropertyMediaQuery(propertyId));
            return Ok(result);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadMediaRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("الملف مطلوب");

            var command = new UploadMediaCommand(
                request.PropertyId,
                request.File.FileName,
                request.File.ContentType,
                request.File.OpenReadStream(),
                request.MediaType,
                request.IsCover,
                request.SortOrder
            );

            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}/cover")]
        public async Task<IActionResult> SetCover(Guid id)
        {
            var result = await _mediator.Send(new SetCoverCommand(id));
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteMediaCommand(id));
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }

    public class UploadMediaRequest
    {
        public Guid PropertyId { get; set; }
        public IFormFile? File { get; set; }
        public Domain.Common.Enums.MediaType MediaType { get; set; } 
        public bool IsCover { get; set; } = false;
        public int SortOrder { get; set; } = 0;
    }
}
