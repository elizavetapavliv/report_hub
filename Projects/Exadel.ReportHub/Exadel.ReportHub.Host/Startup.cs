using Exadel.ReportHub.RA;
using Microsoft.OpenApi.Models;

namespace Exadel.ReportHub.Host;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            const string apiVersion = "v1";

            c.SwaggerDoc(apiVersion, new OpenApiInfo { Title = "ReportHubAPI", Version = apiVersion });
        });

        services.AddAuthorization();
        services.ConfigureInfrastructureServices(_configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Report Hub API"));

        app.UseRouting();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
