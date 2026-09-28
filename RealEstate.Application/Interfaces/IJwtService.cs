using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}