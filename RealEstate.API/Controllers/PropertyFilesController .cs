using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Features.PropertyDocuments.Commands.DeleteDocument;
using RealEstate.Application.Features.PropertyDocuments.Commands.UploadDocument;
using RealEstate.Application.Features.PropertyMedia.Commands.DeleteMedia;
using RealEstate.Application.Features.PropertyMedia.Commands.SetCover;
using RealEstate.Application.Features.PropertyMedia.Commands.UploadMedia;
using RealEstate.Shared.Authorization;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/properties/{propertyId:guid}")]
    [Authorize(Policy = Policies.AgentAndUp)]
    public class PropertyFilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertyFilesController(IMediator mediator) => _mediator = mediator;

        // ── Media ─────────────────────────────────────────

        [HttpPost("media")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadMedia(
            Guid propertyId,
            [FromForm] UploadPropertyMediaCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                cmd, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("media/{mediaId:guid}/cover")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> SetCover(
            Guid propertyId,
            Guid mediaId,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(new SetCoverCommand
            {
                PropertyId = propertyId,
                MediaId = mediaId
            }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("media/{mediaId:guid}")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> DeleteMedia(
            Guid propertyId,
            Guid mediaId,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(new DeleteMediaCommand
            {
                PropertyId = propertyId,
                MediaId = mediaId
            }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ── Documents ─────────────────────────────────────

        [HttpPost("documents")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDocument(
            Guid propertyId,
            [FromForm] UploadPropertyDocumentCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("documents/{documentId:guid}")]
        [Authorize(Policy = Policies.AdminAndUp)]
        public async Task<IActionResult> DeleteDocument(
            Guid propertyId,
            Guid documentId,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(new DeleteDocumentCommand
            {
                PropertyId = propertyId,
                DocumentId = documentId
            }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}