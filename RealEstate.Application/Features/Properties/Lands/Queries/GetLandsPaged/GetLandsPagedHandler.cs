using MediatR;
using RealEstate.Application.Features.Properties.Lands.Dtos;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Lands.Queries.GetLandsPaged
{
    public class GetLandsPagedHandler
        : IRequestHandler<GetLandsPagedQuery, ApiResponse<PagedResult<LandListItemDto>>>
    {
        private readonly ILandRepository _landRepository;

        public GetLandsPagedHandler(ILandRepository landRepository)
        {
            _landRepository = landRepository;
        }

        public async Task<ApiResponse<PagedResult<LandListItemDto>>> Handle(GetLandsPagedQuery request, CancellationToken ct)
        {
            var paged = await _landRepository.GetPagedAsync(request.CompanyId, request.Page, request.PageSize);

            var result = new PagedResult<LandListItemDto>
            {
                Page = paged.Page,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount,
                Items = paged.Items.Select(x => new LandListItemDto
                {
                    Id = x.Id,
                    PropertyCode = x.PropertyCode,
                    Title = x.Title,
                    Price = x.Price,
                    Area = x.Area,
                    City = x.City,
                    District = x.District,
                    PropertyStatus = x.PropertyStatus.ToString(),
                    StreetWidth = x.StreetWidth,
                    ZoningType = x.ZoningType,
                    CornerLand = x.CornerLand,
                    NumberOfStreets = x.NumberOfStreets,
                    OwnerId = x.OwnerId ?? Guid.Empty,
                    AgentId = x.AgentId ?? Guid.Empty
                }).ToList()
            };

            return ApiResponse<PagedResult<LandListItemDto>>.Ok(result);
        }
    }
}
