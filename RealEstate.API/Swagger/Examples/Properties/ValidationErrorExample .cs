using RealEstate.Shared.Common;
using Swashbuckle.AspNetCore.Filters;

namespace RealEstate.API.Swagger.Examples
{
    public class ValidationErrorExample : IExamplesProvider<ApiResponse<object>>
    {
        public ApiResponse<object> GetExamples() =>
            ApiResponse<object>.Fail(new List<string>
            {
                "البريد الإلكتروني مطلوب",
                "كلمة المرور يجب أن تكون 8 أحرف على الأقل"
            });
    }

    public class NotFoundErrorExample : IExamplesProvider<ApiResponse<object>>
    {
        public ApiResponse<object> GetExamples() =>
            ApiResponse<object>.Fail("الشقة بالمعرف '...' غير موجودة");
    }

    public class UnauthorizedErrorExample : IExamplesProvider<ApiResponse<object>>
    {
        public ApiResponse<object> GetExamples() =>
            ApiResponse<object>.Fail("يجب تسجيل الدخول أولاً");
    }

    public class RateLimitErrorExample : IExamplesProvider<ApiResponse<object>>
    {
        public ApiResponse<object> GetExamples() =>
            ApiResponse<object>.Fail(
                "لقد تجاوزت الحد المسموح به من الطلبات، يرجى المحاولة لاحقاً");
    }
}