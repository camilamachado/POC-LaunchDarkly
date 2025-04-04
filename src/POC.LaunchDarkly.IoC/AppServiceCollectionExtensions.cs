using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server;
using LaunchDarkly.Sdk.Server.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using POC.LaunchDarkly.Domain;

namespace POC.LaunchDarkly.IoC;

public static class AppServiceCollectionExtensions
{
	public static void ConfigureAppDependencies(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(IDomainEntryPoint).Assembly));

		ConfigureLaunchDarkly(services, configuration);
	}

	private static void ConfigureLaunchDarkly(IServiceCollection services, IConfiguration configuration)
	{
		var loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
		var logger = loggerFactory.CreateLogger(typeof(AppServiceCollectionExtensions));

		var sdkKey = "sdk-02c8c6c3-c525-4d7f-81c7-90355ec78ca9";

		var ldConfig = Configuration.Builder(sdkKey)
			.ServiceEndpoints(Components.ServiceEndpoints()
			  .Streaming("https://stream.launchdarkly.com")
			  .Polling("https://sdk.launchdarkly.com")
			  .Events("https://events.launchdarkly.com"))

			.Build();

		ILdClient ldClient = new LdClient(ldConfig);

		if (!ldClient.Initialized)
		{
			logger.LogWarning("Erro ao inicializar LaunchDarkly: {Message}", ldClient.DataStoreStatusProvider.Status);
		}

		var ldContext = Context.Builder("poc-launch-darkly-api").Build();

		services.AddSingleton<Lazy<Context>>(serviceProvider => new Lazy<Context>(() => ldContext));
		services.AddSingleton(ldClient);
	}
}
