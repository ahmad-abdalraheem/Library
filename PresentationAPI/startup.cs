using Application.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddControllers(); // Register controllers
		services.AddApplicationServices(); // Register your application services
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		app.UseRouting();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers(); // Map controller routes
		});
	}
}