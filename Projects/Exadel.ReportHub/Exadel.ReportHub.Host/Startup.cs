using AutoMapper;
using Exadel.ReportHub.Host.Filters;
using Exadel.ReportHub.Host.Registrations;
using Microsoft.OpenApi.Models;

namespace Exadel.ReportHub.Host;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ExceptionFilter>();
        });

        services.AddSwaggerGen(c =>
        {
            const string apiVersion = "v1";

            c.SwaggerDoc(apiVersion, new OpenApiInfo { Title = "ReportHubAPI", Version = apiVersion });
        });

        services.AddAuthorization();

        services.AddIdentity();
        services.AddMongo();
        services.AddMediatR();
        services.AddAutoMapper(typeof(Startup));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
    {
        //mapper.ConfigurationProvider.AssertConfigurationIsValid();

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Report Hub API"));
        app.UseRouting();

        app.UseIdentityServer();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
