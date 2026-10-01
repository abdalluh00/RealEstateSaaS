namespace RealEstate.Shared.Settings
{
    public class RateLimitingSettings
    {
        public AuthRateLimits Auth { get; set; } = new();
        public RateLimit General { get; set; } = new();
    }

    public class AuthRateLimits
    {
        public RateLimit Login { get; set; } = new();
        public RateLimit ResetPassword { get; set; } = new();
        public RateLimit ChangePassword { get; set; } = new();
        public RateLimit AcceptInvitation { get; set; } = new();
    }

    public class RateLimit
    {
        public int PermitLimit { get; set; }
        public int WindowSeconds { get; set; }
    }
}