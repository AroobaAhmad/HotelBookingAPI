using System.Text;
using HotelBookingAPI.Models;
using HotelBookingAPI.Repositories;
using HotelBookingAPI.Services.Handlers;
using HotelBookingAPI.Services.HandlerServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HotelBookingAPI
{


    // Add services to the container.
    public class Program
    {
        //public static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(IConfiguration configuration)
        //{
        //    var databaseName = configuration["DatabaseName"];
        //    var containerName = configuration["ContainerName"];
        //    var account = configuration["Account"];
        //    var key = configuration["Key"];
        //    var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
        //    var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
        //    var container = await database.Database.CreateContainerIfNotExistsAsync(containerName,"/category");
        //    var cosmosDbService = new CosmosDbService(client, databaseName, containerName);
        //    return cosmosDbService;

        //}
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var secretKey = builder.Configuration["JwtSettings:Secret"] ?? "ThisIsASecretKeyForJwtTokenGeneration";
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IGenericRepository<Customer>, GenericRepository<Customer>>();
            builder.Services.AddScoped<CommandHandlerService>();
            builder.Services.AddScoped<QueryHandlerService>();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseCosmos(
                    accountEndpoint: builder.Configuration["CosmosDb:Account"],
                    accountKey: builder.Configuration["CosmosDb:Key"],
                    databaseName: builder.Configuration["CosmosDb:DatabaseName"]
                ));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            //app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}