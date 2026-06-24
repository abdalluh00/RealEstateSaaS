using MediatR;
using RealEstate.Application.Features.Properties.Buildings.Dtos;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Buildings.Queries.GetBuildingsPaged
{
    public class GetBuildingsPagedHandler
       : IRequestHandler<GetBuildingsPagedQuery, ApiResponse<PagedResult<BuildingListItemDto>>>
    {
        private readonly IBuildingRepository _buildingRepository;

        public GetBuildingsPagedHandler(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository;
        }

        public async Task<ApiResponse<PagedResult<BuildingListItemDto>>> Handle(GetBuildingsPagedQuery request, CancellationToken ct)
        {
            var paged = await _buildingRepository.GetPagedAsync(request.CompanyId, request.Page, request.PageSize);

            var result = new PagedResult<BuildingListItemDto>
            {
                Page = paged.Page,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount,
                Items = paged.Items.Select(x => new BuildingListItemDto
                {
                    Id = x.Id,
                    PropertyCode = x.PropertyCode,
                    Title = x.Title,
                    Price = x.Price,
                    Area = x.Area,
                    City = x.City,
                    District = x.District,
                    PropertyStatus = x.PropertyStatus.ToString(),
                    TotalFloors = x.TotalFloors,
                    UnitsCount = x.UnitsCount,
                    Elevator = x.Elevator,
                    ParkingFloor = x.ParkingFloor,
                    OwnerId = x.OwnerId ?? Guid.Empty,
                    AgentId = x.AgentId ?? Guid.Empty
                }).ToList()
            };

            return ApiResponse<PagedResult<BuildingListItemDto>>.Ok(result);
        }
    }
}
