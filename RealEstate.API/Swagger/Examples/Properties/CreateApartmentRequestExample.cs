using RealEstate.Application.Features.Properties.Apartments.Commands.CreateApartment;
using RealEstate.Domain.Common.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace RealEstate.API.Swagger.Examples.Properties
{
    public class CreateApartmentRequestExample
        : IExamplesProvider<CreateApartmentCommand>
    {
        public CreateApartmentCommand GetExamples() => new()
        {
            Title = "شقة فاخرة في حي العليا",
            Description = "شقة مميزة بإطلالة رائعة في قلب الرياض",
            Purpose = PropertyPurpose.ForRent,
            Price = 2500,
            Area = 150,
            City = "الرياض",
            District = "العليا",
            Address = "شارع التخصصي، برج الأفق",
            Bedrooms = 3,
            Bathrooms = 2,
            LivingRooms = 1,
            FloorNumber = 5,
            HasElevator = true,
            HasBalcony = true,
            HasCentralAC = true,
            FurnishedStatus = FurnishedStatus.SemiFurnished,
            FacingDirection = FacingDirection.North
        };
    }
}