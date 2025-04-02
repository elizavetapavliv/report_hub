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
            var applicationUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS").Split(';')[0];
            var authorizationUrl = new Uri($"{applicationUrl}/connect/authorize");
            var tokenUrl = new Uri($"{applicationUrl}/connect/token");

            c.SwaggerDoc(apiVersion, new OpenApiInfo { Title = "ReportHubAPI", Version = apiVersion });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = authorizationUrl,
                        TokenUrl = tokenUrl
                    },
                    Password = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = authorizationUrl,
                        TokenUrl = tokenUrl
                    }
                }
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] { }
                }
            });
        });

        services.AddAuthorization();

        services.AddIdentity();
        services.AddMongo();
        services.AddMediatR();
        services.AddAutoMapper(typeof(Startup));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
    {
        mapper.ConfigurationProvider.AssertConfigurationIsValid();

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
