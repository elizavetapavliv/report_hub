﻿using Exadel.ReportHub.Host.Filters;
using Microsoft.OpenApi.Models;

namespace Exadel.ReportHub.Host;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<GlobalExceptionFilter>();
        });

        services.AddSwaggerGen(c =>
        {
            const string apiVersion = "v1";

            c.SwaggerDoc(apiVersion, new OpenApiInfo { Title = "ReportHubAPI", Version = apiVersion });
        });

        services.AddAuthorization();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

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
