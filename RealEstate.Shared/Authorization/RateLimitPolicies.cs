namespace RealEstate.Shared.Authorization
{
    public static class RateLimitPolicies
    {
        public const string Login = "login-limit";
        public const string ResetPassword = "reset-password-limit";
        public const string ChangePassword = "change-password-limit";
        public const string AcceptInvitation = "accept-invitation-limit";
        public const string General = "general-limit";
    }
}