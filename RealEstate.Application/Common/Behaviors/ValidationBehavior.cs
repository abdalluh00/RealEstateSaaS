using FluentValidation;
using MediatR;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TData>
     : IPipelineBehavior<TRequest, ApiResponse<TData>>
     where TRequest : IRequest<ApiResponse<TData>>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<ApiResponse<TData>> Handle(
            TRequest request,
            RequestHandlerDelegate<ApiResponse<TData>> next,
            CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var errors = _validators
                .SelectMany(v => v.Validate(context).Errors)
                .Where(x => x != null)
                .Select(x => x.ErrorMessage)
                .ToList();

            if (errors.Any())
                return ApiResponse<TData>.Fail(errors);

            return await next();
        }
    }
}
