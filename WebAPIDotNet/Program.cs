using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<itiContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));


            });
           
            //////////////////////////////////////////////////////////////////////////////////
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<itiContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               // [authorize]
                options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;  //unauth
                options.DefaultScheme=JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(Options=> //verified key
            {
                Options.SaveToken = true;
                Options.RequireHttpsMetadata = false;
                Options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer= true,
                    ValidIssuer = builder.Configuration["JWT:IssuerIP"],
                    ValidateAudience= true,
                    ValidAudience= builder.Configuration["JWT:AudianceIP"],
                    IssuerSigningKey =
                         new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecrityKey"]))
                };
            });

            //////////////////////////////////////////////////////////////////////////////////
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            // builder.Services.AddSwaggerGen();
            #region Swagger setting 
            builder.Services.AddSwaggerGen(swagger =>
            {
                // This is to generate the Default UI of Swagger Documentation
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET 5 Web API",
                    Description = "ITI Project"
                });

                // To Enable authorization using Swagger (JWT)
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token."
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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policy =>
                policy.AllowAnyOrigin()
                .AllowAnyMethod()
                );
            });
            #endregion 


            var app = builder.Build();

            app.UseCors("MyPolicy");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
           // app.UseAuthentication(); by default { Cookie }
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
