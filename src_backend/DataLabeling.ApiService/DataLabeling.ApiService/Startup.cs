using System.Text;
using DataLabeling.Api.Common.Authentication;
using DataLabeling.Api.Common.Configuration.Extensions;
using DataLabeling.Api.Common.Configuration.Settings;
using DataLabeling.Api.Common.Middleware;
using DataLabeling.ApiService.ConfigurationHelper;
using DataLabeling.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace DataLabeling.ApiService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TokenConfiguration>(Configuration.GetSection(nameof(TokenConfiguration)));
            services.Configure<LiqPayConfiguration>(Configuration.GetSection(nameof(LiqPayConfiguration)));
            services.Configure<EmailSenderConfiguration>(Configuration.GetSection(nameof(EmailSenderConfiguration)));

            var dataBaseConf = Configuration.GetConfiguration<DataBaseConfiguration>();
            services.AddDbContext<DateLabelingDbContext>(options => options.UseSqlServer(dataBaseConf.ConnectionString));

            services.AddDataLabelingServices();

            var tokenConfig = Configuration.GetConfiguration<TokenConfiguration>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidIssuer = tokenConfig.Issuer,
                    ValidAudience = tokenConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.Secret))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyName.ForCustomer, configure =>
                    configure.RequireRole(RoleName.Customer));

                options.AddPolicy(PolicyName.ForPerformer, configure =>
                    configure.RequireRole(RoleName.Performer));
            });

            services.AddCors();
            services.AddControllers();

            services.AddSwaggerGen(conf =>
            {
                conf.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DataLabeling API"
                });

                conf.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                conf.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DateLabelingDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors(options =>
                options.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "DataLabeling API"));

            // context.Database.EnsureDeleted();
            context.Database.Migrate();
        }
    }
}
