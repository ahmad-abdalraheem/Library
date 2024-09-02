using Application.MappingProfile;

public static class Configuration
{
	public static void RegisterServices(this WebApplicationBuilder builder)
	{
		builder.Services
			.AddEndpointsApiExplorer()
			.AddSwaggerGen()
			.AddApplicationServices()
			.AddAutoMapper(typeof(BookProfile).Assembly);
	}

	public static void RegisterMiddlewares(this WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger()
				.UseSwaggerUI();
		}

		app.UseHttpsRedirection();
	}
}