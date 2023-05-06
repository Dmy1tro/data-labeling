using DataLabeling.Common.Shared;
using DataLabeling.Common.User;
using DataLabeling.Entities;
using DataLabeling.Services.Interfaces.User.Models;
using DataLabeling.Services.User.Models;
using System;

namespace DataLabeling.Services.User.Mappers
{
    public static class UserMappingExtensions
    {
        public static IUserModel MapToModel(this Entities.User user)
        {
            return new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailConfirmed = user.EmailConfirmed,
                PinConfirmation = user.PinConfirmation,
                UserType = user.GetUserType(),
                Roles = user.Roles
            };
        }

        public static IPerformerModel MapToPerformerModel(this Performer performer)
        {
            return new PerformerModel
            {
                Id = performer.Id,
                Email = performer.Email,
                FirstName = performer.FirstName,
                LastName = performer.LastName,
                EmailConfirmed = performer.EmailConfirmed,
                PinConfirmation = performer.PinConfirmation,
                UserType = performer.GetUserType(),
                Roles = performer.Roles,
                Balace = performer.Balance,
                Rating = performer.Rating
            };
        }

        public static Performer MapToPerformerEntity(this IRegisterModel registerModel, string passwordHash)
        {
            return new Performer
            {
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Balance = 0m,
                Rating = Rating.Normal,
                Roles = registerModel.Roles,
                PasswordHash = passwordHash
            };
        }

        public static Customer MapToCustomerEntity(this IRegisterModel registerModel, string passwordHash)
        {
            return new Customer
            {
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Roles = registerModel.Roles,
                PasswordHash = passwordHash
            };
        }

        public static UserType GetUserType(this Entities.User user)
        {
            return user switch
            {
                Customer => UserType.Customer,

                Performer => UserType.Performer,

                _ => throw new ArgumentException(nameof(user))
            };
        }
    }
}
