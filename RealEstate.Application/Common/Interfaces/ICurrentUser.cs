namespace RealEstate.Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        Guid CompanyId { get; }
        string Role { get; }
        string Email { get; }
        bool IsAuthenticated { get; }
    }
}
