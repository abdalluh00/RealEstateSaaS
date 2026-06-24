using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Features.Companies.Commands.CreateCompany;
using RealEstate.Application.Features.Companies.Commands.DeleteCompany;
using RealEstate.Application.Features.Companies.Commands.UpdateCompany;
using RealEstate.Application.Features.Companies.Queries.GetCompanies;
using RealEstate.Application.Features.Companies.Queries.GetCompanyById;
using RealEstate.Shared.Authorization;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompaniesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetCompaniesQuery());
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCompanyByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCompanyCommand command)
        {
            if (id != command.Id) return BadRequest("المعرف غير متطابق");
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteCompanyCommand(id));
            return result.Success ? Ok(result) : BadRequest(result);
        }


        // جيب بيانات الشركة من الـ Token
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var companyId = Guid.Parse(
                User.FindFirst("CompanyId")!.Value);
            var result = await _mediator.Send(
                new GetCompanyByIdQuery(companyId));
            return Ok(result);
        }

        // تعديل بيانات الشركة
        [HttpPut("me")]
        [Authorize(Policy = Policies.OwnerOnly)]
        public async Task<IActionResult> UpdateMe(
            [FromBody] UpdateCompanyMeCommand command)
        {
            var companyId = Guid.Parse(
                User.FindFirst("CompanyId")!.Value);
            var result = await _mediator.Send(
                command with { Id = companyId });
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
