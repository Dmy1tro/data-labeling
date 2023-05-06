using DataLabeling.Common.Shared;
using DataLabeling.Common.User;
using DataLabeling.Services.Interfaces.User.Models;
using System.Threading.Tasks;

namespace DataLabeling.DAL.Services.Interfaces.User.Services
{
    public interface IUserService
    {
        Task<IAuthenticatedResult> Register(IRegisterModel registerModel);

        Task<IAuthenticatedResult> Authenticate(string email, string password, UserType userType);

        Task ConfirmEmailAsync(string email, string pin, UserType userType);

        Task RefreshPerformerRating(int performerId);

        Task AddToPerformerBalance(int performerId, decimal sallary);

        Task<IPerformerModel> GetPerformerAsync(int performerId);

        Task<ICustomerStatistic> GetCustomerStatisticAsync(int customerId);

        Task<IPerformerStatistic> GetPerformerStatisticAsync(int performerId);
    }
}
