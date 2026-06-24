namespace RealEstate.Shared.Common.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "غير مصرح لك بالوصول")
            : base(message) { }
    }
}
