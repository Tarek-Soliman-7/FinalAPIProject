using E_Commerce.API.Extentions;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Persistence.Identity;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region DI Container
            // Web API services
            builder.Services.AddWepApiServices();

            // Infrastructure services
            builder.Services.AddInfrastructureServices(builder.Configuration);


            // Core services
            builder.Services.AddCoreServices(builder.Configuration);

            // Swagger settings
            builder.Services.AddSwaggerGen(c =>
            {
                c.UseInlineDefinitionsForEnums();
                c.SchemaFilter<DisplayEnumSchemaFilter>();
            });
            #endregion


            #region Pipelines - Middlewares
            var app = builder.Build();

            await app.SeedDatabaseAsync();

            app.UseExceptionHandlingMiddlewares();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

           
            app.UseAuthentication();
            app.UseAuthorization();//step 1

            app.MapControllers();//step 2

            app.Run();
            #endregion
        }
    }
}
