using DataLabeling.Common.Order;
using DataLabeling.Common.Shared;
using DataLabeling.Common.User;
using DataLabeling.DAL.Services.Interfaces.User.Services;
using DataLabeling.Data.Context;
using DataLabeling.Entities;
using DataLabeling.Helpers.Password;
using DataLabeling.Infrastructure.Guard;
using DataLabeling.Services.Interfaces.User.Models;
using DataLabeling.Services.User.Mappers;
using DataLabeling.Services.User.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataLabeling.Services.User.Services
{
    public class UserService : IUserService
    {
        private readonly DateLabelingDbContext _dbContext;

        public UserService(DateLabelingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddToPerformerBalance(int performerId, decimal sallary)
        {
            var performer = await _dbContext.Performers.FindAsync(performerId);
            Guard.ObjectFound(performer);

            performer.Balance += sallary;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IAuthenticatedResult> Authenticate(string email, string password, UserType userType)
        {
            Guard.PropertyNotEmpty(email, nameof(email));
            Guard.PropertyNotEmpty(password, nameof(password));

            var result = new AuthenticatedResult { IsSuccess = true };
            var user = await GetUserAsync(email, userType);

            if (user is not null && !user.EmailConfirmed)
            {
                result.IsSuccess = false;
                result.FailureReason = "Email not confirmed.";
                return result;
            }

            if (user is null || !PasswordHasher.Verify(password, user.PasswordHash))
            {
                result.IsSuccess = false;
                result.FailureReason = "Invalid login attempt.";
                return result;
            }

            result.User = user.MapToModel();

            return result;
        }

        public async Task<IAuthenticatedResult> Register(IRegisterModel registerModel)
        {
            var result = new AuthenticatedResult { IsSuccess = true };
            var userExists = await GetUserAsync(registerModel.Email, registerModel.UserType);

            if (userExists is not null)
            {
                result.IsSuccess = false;
                result.FailureReason = "User with this email already exists.";
                return result;
            }

            var passwordHash = PasswordHasher.Hash(registerModel.Password);

            Entities.User createdUser = default;
            if (registerModel.UserType == UserType.Customer)
            {
                var customer = registerModel.MapToCustomerEntity(passwordHash);
                _dbContext.Customers.Add(customer);
                createdUser = customer;
            }

            else if (registerModel.UserType == UserType.Performer)
            {
                var performer = registerModel.MapToPerformerEntity(passwordHash);
                _dbContext.Performers.Add(performer);
                createdUser = performer;
            }

            createdUser.PinConfirmation = Guid.NewGuid().ToString();
            await _dbContext.SaveChangesAsync();

            result.User = createdUser.MapToModel();

            return result;
        }

        public async Task RefreshPerformerRating(int performerId)
        {
            var performer = await _dbContext.Performers
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == performerId);

            Guard.ObjectFound(performer);

            var ratingScore = performer.Reviews
                .Select(r => (int)r.Rating)
                .Union(new[] { (int)Rating.Normal })
                .Average();

            performer.Rating = (Rating)Convert.ToInt32(ratingScore);

            await _dbContext.SaveChangesAsync();
        }

        public async Task ConfirmEmailAsync(string email, string pin, UserType userType)
        {
            var user = await GetUserAsync(email, userType);
            Guard.ObjectFound(user);

            if (user.PinConfirmation != pin)
            {
                throw new Common.Exceptions.ValidationException("Confirmation pin is incorrect");
            }

            user.EmailConfirmed = true;
            
            if (userType == UserType.Customer)
            {
                _dbContext.Customers.Update((Customer)user);
            } 
            if (userType == UserType.Performer)
            {
                _dbContext.Performers.Update((Performer)user);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICustomerStatistic> GetCustomerStatisticAsync(int customerId)
        {
            var customer = await _dbContext.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == customerId);
            Guard.ObjectFound(customer);

            var rawOrderCount = await _dbContext.Orders.CountAsync(o => o.Type == Common.Order.OrderType.CollectData);
            var labelOrderCount = await _dbContext.Orders.CountAsync(o => o.Type == Common.Order.OrderType.LabelData);

            return new CustomerStatistic { RawDataOrders = rawOrderCount, LabelDataOrders = labelOrderCount };
        }

        public async Task<IPerformerStatistic> GetPerformerStatisticAsync(int performerId)
        {
            var performer = await _dbContext.Performers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == performerId);
            Guard.ObjectFound(performer);

            var rawDataJobs = await _dbContext.DataSet.CountAsync(d => d.Order.Type == OrderType.CollectData &&
                                                                       d.PerformerId == performerId);

            var labelDataJobs = await _dbContext.DataSet.CountAsync(d => d.Order.Type == OrderType.LabelData &&
                                                                         d.PerformerId == performerId);

            return new PerformerStatistic { RawDataCount = rawDataJobs, LabelDataCount = labelDataJobs };
        }

        public async Task<IPerformerModel> GetPerformerAsync(int performerId)
        {
            var performer = await _dbContext.Performers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == performerId);
            Guard.ObjectFound(performer);

            return performer.MapToPerformerModel();
        }

        private async Task<Entities.User> GetUserAsync(string email, UserType userType)
        {
            if (userType == UserType.Customer)
            {
                return await _dbContext.Customers.FirstOrDefaultAsync(x => x.Email == email);
            }

            if (userType == UserType.Performer)
            {
                return await _dbContext.Performers.FirstOrDefaultAsync(x => x.Email == email);
            }

            throw new ArgumentException(nameof(userType));
        }
    }
}
