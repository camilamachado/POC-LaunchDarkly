using Microsoft.EntityFrameworkCore;
using POC.LaunchDarkly.Api.Endpoints;
using POC.LaunchDarkly.Api.Middlewares;
using POC.LaunchDarkly.Data;

namespace POC.LaunchDarkly.Api.Extensions;

public static class AppConfigureExtensions
{
	public static void Configure(this WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			using (var scope = app.Services.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

				if (db.Database.IsRelational())
					db.Database.Migrate();
			}
		}

		app.UseMiddleware<ExceptionMiddleware>();

		app.MapDefaultEndpoints();
		app.MapEmprestimoEndpoints();

		app.MapOpenApi();
		app.UseSwagger();
		app.UseSwaggerUI();
	}
}
