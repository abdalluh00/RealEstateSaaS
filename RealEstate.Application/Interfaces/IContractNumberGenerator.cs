namespace RealEstate.Application.Common.Interfaces
{
    public interface IContractNumberGenerator
    {
        Task<string> GenerateAsync(Guid companyId, CancellationToken ct = default);
    }
}