namespace RealEstate.Application.Common.Interfaces
{
    public interface IMaintenanceNumberGenerator
    {
        Task<string> GenerateAsync(Guid companyId, CancellationToken ct = default);
    }
}