using DataLabeling.Api.Common.Configuration.Settings;
using DataLabeling.Infrastructure.Date;
using DataLabeling.Services.Interfaces.Jwt;
using DataLabeling.Services.Interfaces.User.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DataLabeling.ApiService.User.Services
{
    public class JwtService : IJwtService
    {
        private readonly TokenConfiguration _tokenSettings;
        private readonly IDateTimeProvider _dateTimeProvider;

        public JwtService(IOptions<TokenConfiguration> tokenConfiguration, IDateTimeProvider dateTimeProvider)
        {
            _tokenSettings = tokenConfiguration.Value;
            _dateTimeProvider = dateTimeProvider;
        }

        public string GenerateToken(IUserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.UserType.ToString())
            };

            var expires = _dateTimeProvider.UtcNow.AddDays(1);
            var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret));
            var credentials = new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _tokenSettings.Issuer,
                audience: _tokenSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
