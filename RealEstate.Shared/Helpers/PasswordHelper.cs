
namespace RealEstate.Shared.Helpers
{
    public static class PasswordHelper
    {
        public static string Hash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password);

        public static bool Verify(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch
            {
                // الـ Hash قديم أو غير صحيح → نرجع false بدل Exception
                return false;
            }
        }
    }
}
