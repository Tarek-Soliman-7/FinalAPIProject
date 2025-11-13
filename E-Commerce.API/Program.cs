using E_Commerce.API.Extentions;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Persistence.Identity;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region DI Container
            // Web API services
            builder.Services.AddWepApiServices(builder.Configuration);

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
                app.UseSwagger();
                app.UseSwaggerUI( op =>
                {
                    op.ConfigObject = new ConfigObject()
                    {
                        DisplayRequestDuration=true,
                    };

                    op.DocumentTitle = "My E-Commerce API";

                    op.JsonSerializerOptions = new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    op.DocExpansion(DocExpansion.None);

                    op.EnableFilter();

                    op.EnablePersistAuthorization();
                });
                
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");
           
            app.UseAuthentication();
            app.UseAuthorization();//step 1

            app.MapControllers();//step 2

            app.Run();
            #endregion
        }
    }
}
