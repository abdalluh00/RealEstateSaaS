using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Domain.Interfaces.Properties
{

   
        public interface IOfficeRepository : IGenericRepository<OfficeProperty>
        {
            Task<OfficeProperty?> GetByIdForCompanyAsync(
                Guid officeId,
                Guid companyId,
                CancellationToken ct = default);
        }
    }
