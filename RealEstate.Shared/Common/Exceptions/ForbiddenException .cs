namespace RealEstate.Shared.Common.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message = "ليس لديك صلاحية لهذا الإجراء")
            : base(message) { }
    }
}
