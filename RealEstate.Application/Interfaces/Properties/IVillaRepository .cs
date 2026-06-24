using RealEstate.Domain.Entities.Properties;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.Interfaces.Properties
{
    // IVillaRepository
    public interface IVillaRepository
    {
        Task<VillaDetailDto?> GetByIdAsync(Guid id, Guid companyId, CancellationToken ct = default);
        Task<PagedResult<VillaListDto>> GetPagedAsync(...);
        Task AddAsync(VillaProperty villa, CancellationToken ct = default);
        void Update(VillaProperty villa);
    }
}
