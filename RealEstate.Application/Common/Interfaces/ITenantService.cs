
namespace RealEstate.Application.Common.Interfaces
{
    public interface ITenantService
    {
        Guid CompanyId { get; }
        string Role { get; }
        Guid UserId { get; }
        bool IsOwner { get; }
        bool IsAdmin { get; }
        bool IsAgent { get; }
    }
}
