using DataLabeling.Services.Interfaces.User.Models;

namespace DataLabeling.Services.Interfaces.Jwt
{
    public interface IJwtService
    {
        string GenerateToken(IUserModel userModel);
    }
}
