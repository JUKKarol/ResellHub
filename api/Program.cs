using FluentValidation;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ResellHub.Data;
using ResellHub.Data.Repositories.OfferRepository;
using ResellHub.Data.Repositories.UserRepository;
using ResellHub.Data.Seeders;
using ResellHub.DTOs.OfferDTOs;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Services.EmailService;
using ResellHub.Services.OfferServices;
using ResellHub.Services.UserServices;
using ResellHub.Utilities.OfferUtilities;
using ResellHub.Utilities.UserUtilities;
using ResellHub.Utilities.Validation.Offer;
using ResellHub.Utilities.Validation.UserValidation;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ResellHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            builder.Services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            builder.Configuration.GetSection("AppSettings:Token").Value!))
                };
            });

            builder.Services.AddDbContext<ResellHubContext>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<IValidator<OfferCreateDto>, OfferCreateValidator>();
            builder.Services.AddScoped<IValidator<UserRegistrationDto>, UserRegistrationValidation>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOfferRepository, OfferRepository>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IOfferService, OfferService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddScoped<IUserUtilities, UserUtilities>();
            builder.Services.AddScoped<IOfferUtilities, OfferUtilities>();

            var app = builder.Build();
            var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<ResellHubContext>();
            var userUtilities = scope.ServiceProvider.GetService<IUserUtilities>();

            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                dbContext.Database.Migrate();
            }

            var seeder = new Seeder(dbContext, userUtilities);
            seeder.Seed();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}