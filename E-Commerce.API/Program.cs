
using Domain.Contracts;
using E_Commerce.API.Factories;
using E_Commerce.API.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presistence.Data;
using Presistence.Repositories;
using Services;
using Services.Abstraction.Contracts;
using Services.Implementations;
using Services.MappingProfiles;
using Shared.Enums;


namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
            }); 

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); 
            });
            builder.Services.AddScoped<IDataSeeding,DataSeeding>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //builder.Services.AddAutoMapper(x => x.AddProfile(new ProductProfile()));
            builder.Services.AddAutoMapper(x => { }, typeof(AssemblyReference).Assembly);
            builder.Services.AddScoped<IServiceManger,ServiceManger>();

            builder.Services.AddSwaggerGen(c =>
            {
                c.UseInlineDefinitionsForEnums(); // يعمل تأثير مشابه
                c.SchemaFilter<DisplayEnumSchemaFilter>();
            });



            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var objOfDataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await objOfDataSeeding.SeedDataAsync();

            //Middleware ==> Handle exceptions
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();      // Middlewares ==> Swagger
                app.UseSwaggerUI();    // Middlewares ==> Swagger
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
