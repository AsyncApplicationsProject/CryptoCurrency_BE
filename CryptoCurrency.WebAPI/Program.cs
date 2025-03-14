using CryptoCurrency.DAL.EF;
using CryptoCurrency.DAL.Seed;
using CryptoCurrency.Model.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Add secrets handler
        builder.Configuration.AddUserSecrets<Program>();

        // DbContext configuration
        builder.Services.AddDbContextPool<AppDbContext>(options =>
                        options.UseSqlServer(builder.Configuration["ConnectionStrings:CryptoCurrencyDb"]
                     ?? throw new InvalidOperationException("Database connection string not configured.")));

        // Swagger configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
            xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));

            // Swagger JWT
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Please enter only '[jwt]'",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Name = "Authorization",
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
        });

        // Dependency Injection configuration
        CryptoCurrency.Services.Configuration.Dependencies.Register(builder.Services);

        // JWT Authentication configuration
        builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        var secret = builder.Configuration["JWT:Secret"] ?? throw new InvalidOperationException("Secret not configured");
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                ValidAudience = builder.Configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ClockSkew = TimeSpan.FromSeconds(5),
                RoleClaimType = ClaimTypes.Role
            };
            options.Events = new JwtBearerEvents
            {
                OnChallenge = ctx => LogAttempt(ctx.Request.Headers, "OnChallenge"),
                OnTokenValidated = ctx => LogAttempt(ctx.Request.Headers, "OnTokenValidated")
            };
        });

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCors(opt =>
        {
            opt.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        var app = builder.Build();

        app.UseCors("AllowAllOrigins");

        // Seed Data configuration
        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            await Seed.Init(serviceProvider);
        }

        // Configure the HTTP request pipeline.
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

        Task LogAttempt(IHeaderDictionary headers, string eventType)
        {
            // var logger = loggerFactory.CreateLogger<Program>();

            var authorizationHeader = headers["Authorization"].FirstOrDefault();

            if (authorizationHeader is null)
            {
                // logger.LogInformation($"{eventType}. JWT not present");
            }
            else
            {
                string jwtString = authorizationHeader.Substring("Bearer ".Length);

                var jwt = new JwtSecurityToken(jwtString);

                // logger.LogInformation($"{eventType}. Expiration: {jwt.ValidTo.ToLongTimeString()}. System time: {DateTime.UtcNow.ToLongTimeString()}");
            }
            return Task.CompletedTask;
        }
    }
}