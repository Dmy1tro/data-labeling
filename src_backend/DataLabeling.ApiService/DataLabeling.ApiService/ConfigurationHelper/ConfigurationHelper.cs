using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using DataLabeling.Api.Common.Authentication;
using DataLabeling.Api.Common.Configuration.Settings;
using DataLabeling.ApiService.User.Services;
using DataLabeling.DAL.Services.Interfaces.User.Services;
using DataLabeling.Infrastructure.Date;
using DataLabeling.Infrastructure.Email;
using DataLabeling.Services.Data.Services;
using DataLabeling.Services.FileStorage.Services;
using DataLabeling.Services.Interfaces.Data.Services;
using DataLabeling.Services.Interfaces.FileStorage.Services;
using DataLabeling.Services.Interfaces.Jwt;
using DataLabeling.Services.Interfaces.Order.Services;
using DataLabeling.Services.Interfaces.Payment.Services;
using DataLabeling.Services.Interfaces.Review.Services;
using DataLabeling.Services.Interfaces.Sallary.Services;
using DataLabeling.Services.Order.Services;
using DataLabeling.Services.Payment.Services;
using DataLabeling.Services.Review.Services;
using DataLabeling.Services.Sallary.Services;
using DataLabeling.Services.User.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DataLabeling.ApiService.ConfigurationHelper
{
    public static class ConfigurationHelper
    {
        public static IServiceCollection AddDataLabelingServices(this IServiceCollection services)
        {
            services.AddScoped<IDataService, DataService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPriceService, PriceService>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailSender, EmailSender>(sp =>
            {
                var emailConfig = sp.GetRequiredService<IOptions<EmailSenderConfiguration>>().Value;
                return new EmailSender(new EmailConfig(emailConfig.DisplayName, emailConfig.UserName, emailConfig.Password));
            });
            services.AddTransient<IFileStorage>(provider =>
            {
                var hostEnvironment = provider.GetRequiredService<IWebHostEnvironment>();
                var rootPath = $"{hostEnvironment.WebRootPath}{Path.DirectorySeparatorChar}Images{Path.DirectorySeparatorChar}";

                return new FileStorage(rootPath);
            });

            services.AddHttpContextAccessor();
            services.AddTransient<IUserAccessor>(provider =>
            {
                var userAccessor = new UserAccessor();
                var context = provider.GetRequiredService<IHttpContextAccessor>().HttpContext;

                var claims = context.User?.Claims;

                if (claims is not null && claims.Any())
                {
                    userAccessor.User = new UserTokenData
                    {
                        Id = int.Parse(GetClaimValue(claims, ClaimTypes.NameIdentifier)),
                        FullName = GetClaimValue(claims, ClaimTypes.Name),
                        Role = GetClaimValue(claims, ClaimTypes.Role)
                    };
                }

                return userAccessor;

                string GetClaimValue(IEnumerable<Claim> claims, string claimType)
                {
                    return claims.FirstOrDefault(c => c.Type.Equals(claimType, StringComparison.InvariantCultureIgnoreCase)).Value;
                }
            });

            return services;
        }
    }
}
