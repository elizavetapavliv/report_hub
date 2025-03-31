namespace Exadel.ReportHub.IDServer;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication();
        services.AddAuthorization();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapControllers();
        //});
    }
}
