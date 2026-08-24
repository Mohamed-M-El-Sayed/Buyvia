using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineStore.API.Middlewares;
using OnlineStore.Infrastructure.DependencyInjection;
using Serilog;

namespace OnlineStore.API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddPresentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters
                        .Add(new JsonStringEnumConverter());
                });

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(cfg =>
            {
                var xmlFile =
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                var xmlPath =
                    Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                {
                    cfg.IncludeXmlComments(xmlPath);
                }

                cfg.AddSecurityDefinition("bearerAuth",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Description = "Enter 'Bearer {token}'"
                    });

                cfg.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "bearerAuth"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultScheme =
                    JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer =
                            builder.Configuration["JwtSettings:Issuer"],

                        ValidAudience =
                            builder.Configuration["JwtSettings:Audience"],

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    builder.Configuration["JwtSettings:Key"]!))
                    };
            });

            builder.Services.AddOutputCache();

            builder.Services.AddCustomRateLimiting();

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddTransient<ErrorHandlingMiddleware>();

            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(
                    context.Configuration);
            });
        }
    }
}