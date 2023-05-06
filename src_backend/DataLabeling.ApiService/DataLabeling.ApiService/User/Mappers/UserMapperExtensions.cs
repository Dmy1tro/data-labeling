using DataLabeling.Api.Common.Authentication;
using DataLabeling.ApiService.User.Models;
using DataLabeling.Services.Interfaces.User.Models;
using DataLabeling.Services.User.Models;

namespace DataLabeling.ApiService.User.Mappers
{
    public static class UserMapperExtensions
    {
        public static LoginResponse ToLoginResponseModel(this IAuthenticatedResult authenticatedResult, string token)
        {
            return new LoginResponse
            {
                Email = authenticatedResult.User.Email,
                FullName = authenticatedResult.User.FullName,
                Roles = authenticatedResult.User.Roles,
                UserType = (int)authenticatedResult.User.UserType,
                Token = token,
            };
        }

        public static IRegisterModel ToRegisterModel(this RegisterRequest registerRequest)
        {
            return new RegisterModel
            {
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Password = registerRequest.Password,
                Roles = registerRequest.UserType == Common.User.UserType.Customer ? RoleName.Customer : RoleName.Performer,
                UserType = registerRequest.UserType
            };
        }

        public static PerformerModelResponse ToResponse(this IPerformerModel model)
        {
            return new PerformerModelResponse
            {
                Email = model.Email,
                FullName = model.FullName,
                Roles = model.Roles,
                UserType = (int)model.UserType,
                Balance = model.Balace,
                Rating = model.Rating
            };
        }
    }
}
