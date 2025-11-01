namespace E_Commerce.API.Extentions
{
    public static class WebAppExtentions
    {
        public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
        {

            using var scope = app.Services.CreateScope();
            var objOfDataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await objOfDataSeeding.SeedDataAsync();
            await objOfDataSeeding.SeedIdentityDataAsync();
            return app;
        }

        public static WebApplication UseExceptionHandlingMiddlewares(this WebApplication app)
        {

            //Middleware ==> Handle exceptions
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            return app;
        }

        public static WebApplication UseSwaggerMiddlewares(this WebApplication app)
        {
            app.UseSwagger();      // Middlewares ==> Swagger
            app.UseSwaggerUI();    // Middlewares ==> Swagger
            return app;
        }
    }
}
