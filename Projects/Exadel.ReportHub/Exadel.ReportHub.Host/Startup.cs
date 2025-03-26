using Exadel.ReportHub.Host.Deployment.Mongo;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace Exadel.ReportHub.Host;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            const string apiVersion = "v1";

            c.SwaggerDoc(apiVersion, new OpenApiInfo { Title = "ReportHubAPI", Version = apiVersion });
        });

        services.AddAuthorization();

        var connectionString = Configuration["MongoDbSettings:ConnectionString"];
        services.AddSingleton<IMongoClient>(new MongoClient(connectionString));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMongoClient mongoClient)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Report Hub API"));

        app.UseRouting();

        var nameDb = Configuration["MongoDbSettings:DatabaseName"];
        InvoiceSeeder.ExecuteSeedScriptAsync(mongoClient, nameDb).GetAwaiter().GetResult();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
