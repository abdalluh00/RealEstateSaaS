using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.API.Swagger.Examples.Properties;
using RealEstate.Application.DTOs.Properties.Apartment;
using RealEstate.Application.Features.Properties.Apartments.Commands.CreateApartment;
using RealEstate.Application.Features.Properties.Apartments.Commands.DeleteApartment;
using RealEstate.Application.Features.Properties.Apartments.Commands.UpdateApartment;
using RealEstate.Application.Features.Properties.Apartments.Queries.GetApartmentById;
using RealEstate.Application.Features.Properties.Apartments.Queries.GetApartments;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;
using Swashbuckle.AspNetCore.Filters;

namespace RealEstate.API.Controllers
{
    /// <summary>
    /// إدارة الشقق السكنية
    /// </summary>
    [ApiController]
    [Route("api/apartments")]
    [Authorize(Policy = Policies.AgentAndUp)]
    [Produces("application/json")]
    public class ApartmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApartmentController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// الحصول على قائمة الشقق مع فلترة وترقيم الصفحات
        /// </summary>
        /// <remarks>
        /// يعيد قائمة مرقمة من الشقق مع دعم الفلترة بالحالة والغرض وعدد الغرف والتأثيث.
        /// مرتبة تنازلياً: المميزة أولاً ثم الأحدث.
        /// </remarks>
        /// <param name="page">رقم الصفحة — يبدأ من 1</param>
        /// <param name="pageSize">عدد العناصر في الصفحة — بحد أقصى 100</param>
        /// <param name="status">فلترة بالحالة: Available, Rented, Sold, Reserved</param>
        /// <param name="purpose">فلترة بالغرض: ForRent, ForSale</param>
        /// <param name="minBedrooms">الحد الأدنى لعدد غرف النوم</param>
        /// <param name="maxBedrooms">الحد الأقصى لعدد غرف النوم</param>
        /// <param name="furnishedStatus">فلترة بحالة التأثيث</param>
        /// <param name="ct">رمز الإلغاء</param>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<ApartmentListDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        [ProducesResponseType(typeof(ApiResponse<object>), 403)]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] PropertyStatus? status = null,
            [FromQuery] PropertyPurpose? purpose = null,
            [FromQuery] int? minBedrooms = null,
            [FromQuery] int? maxBedrooms = null,
            [FromQuery] FurnishedStatus? furnishedStatus = null,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetPagedApartmentsQuery
            {
                Page = page,
                PageSize = pageSize,
                Status = status,
                Purpose = purpose,
                MinBedrooms = minBedrooms,
                MaxBedrooms = maxBedrooms,
                FurnishedStatus = furnishedStatus
            }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// الحصول على تفاصيل شقة محددة
        /// </summary>
        /// <remarks>
        /// يعيد جميع تفاصيل الشقة بما فيها معلومات المالك والوكيل والوسائط والمستندات.
        /// </remarks>
        /// <param name="id">معرف الشقة</param>
        /// <param name="ct">رمز الإلغاء</param>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<ApartmentDetailDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        public async Task<IActionResult> GetDetail(
            Guid id,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetApartmentDetailQuery { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// إنشاء شقة جديدة
        /// </summary>
        /// <remarks>
        /// ينشئ شقة جديدة بحالة "متاح" تلقائياً.
        /// رقم العقار يُولَّد تلقائياً بالتنسيق: PROP-2024-0001.
        ///
        /// **الصلاحية المطلوبة:** Admin أو Owner فقط.
        /// </remarks>
        /// <param name="cmd">بيانات الشقة</param>
        /// <param name="ct">رمز الإلغاء</param>
        [HttpPost]
        [Authorize(Policy = Policies.AdminAndUp)]
        [SwaggerRequestExample(typeof(CreateApartmentCommand),
            typeof(CreateApartmentRequestExample))]
        [ProducesResponseType(typeof(ApiResponse<Guid>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        [ProducesResponseType(typeof(ApiResponse<object>), 403)]
        public async Task<IActionResult> Create(
            [FromBody] CreateApartmentCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// تحديث بيانات شقة
        /// </summary>
        /// <remarks>
        /// تحديث كامل لبيانات الشقة — جميع الحقول مطلوبة.
        ///
        /// **الصلاحية المطلوبة:** Admin أو Owner فقط.
        /// </remarks>
        /// <param name="id">معرف الشقة</param>
        /// <param name="cmd">البيانات المحدثة</param>
        /// <param name="ct">رمز الإلغاء</param>
        [HttpPut("{id:guid}")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 403)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateApartmentCommand cmd,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// حذف شقة (حذف ناعم)
        /// </summary>
        /// <remarks>
        /// يُخفي الشقة من النظام دون حذفها من قاعدة البيانات نهائياً.
        ///
        /// **الصلاحية المطلوبة:** Admin أو Owner فقط.
        /// </remarks>
        /// <param name="id">معرف الشقة</param>
        /// <param name="ct">رمز الإلغاء</param>
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = Policies.AdminAndUp)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 403)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new DeleteApartmentCommand { Id = id }, ct);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}