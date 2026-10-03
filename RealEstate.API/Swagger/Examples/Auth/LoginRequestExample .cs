using RealEstate.Application.Features.Auth.Commands.Login;
using Swashbuckle.AspNetCore.Filters;

namespace RealEstate.API.Swagger.Examples.Auth
{
    public class LoginRequestExample : IExamplesProvider<LoginCommand>
    {
        public LoginCommand GetExamples() => new()
        {
            Email = "owner.basic@realestate.test",
            Password = "Test@1234"
        };
    }
}