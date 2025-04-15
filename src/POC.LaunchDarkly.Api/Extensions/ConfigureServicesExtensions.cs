using POC.LaunchDarkly.IoC;

namespace POC.LaunchDarkly.Api.Extensions;

public static class ConfigureServicesExtensions
{
	public static void ConfigureServices(this IServiceCollection services, ConfigurationManager configuration)
	{
		services.AddOpenApi();
		services.AddSwaggerGen();

		services.ConfigureAppDependencies(configuration);
	}
}
