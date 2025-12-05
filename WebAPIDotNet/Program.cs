using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebAPIDotNet.Model;

namespace WebAPIDotNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Register EF Core with SQL Server
            builder.Services.AddDbContext<itiContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });

            ////////////////////////////////////////////////////////////////////////////

            // Add Identity (Users + Roles)
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<itiContext>();

            // Configure Authentication (JWT Bearer)
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options => // validating token
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:IssuerIP"],

                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:AudianceIP"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecurityKey"]))
                };
            });

            ////////////////////////////////////////////////////////////////////////////

            // Swagger / OpenAPI
            builder.Services.AddEndpointsApiExplorer();

            // builder.Services.AddSwaggerGen();
            #region Swagger settings
            builder.Services.AddSwaggerGen(swagger =>
            {
                // Base info
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET Web API",
                    Description = "ITI Project"
                });

                // Enable JWT Authentication in Swagger
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer <your-token>"
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] {}
                    }
                });
            });
            #endregion

            // Enable CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                );
            });

            var app = builder.Build();

            app.UseCors("MyPolicy");

            // Development environment Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Use Authentication & Authorization
            // app.UseAuthentication(); // for cookie auth (not used here)
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
