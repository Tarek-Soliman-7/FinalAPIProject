
using E_Commerce.API.Extentions;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region DI Contianer
            //Web Api Services
            builder.Services.AddWepApiServices();

            //Infrastructure Services
            builder.Services.AddInfrastructureServices(builder.Configuration);

            //Core Services
            builder.Services.AddCoreServices();

            builder.Services.AddSwaggerGen(c =>
            {
                c.UseInlineDefinitionsForEnums(); // يعمل تأثير مشابه
                c.SchemaFilter<DisplayEnumSchemaFilter>();
            });
            #endregion



            #region Pipelines - Middlewares
            var app = builder.Build();

            await app.SeedDatabaseAsync();

            app.UseExceptionHandlingMiddlewares();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();

            app.Run(); 
            #endregion
        }
    }
}
