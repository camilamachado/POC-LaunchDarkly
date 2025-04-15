using FluentValidation;
using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server;
using LaunchDarkly.Sdk.Server.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using POC.LaunchDarkly.Data;
using POC.LaunchDarkly.Data.Repositories;
using POC.LaunchDarkly.Domain;
using POC.LaunchDarkly.Domain.Repositories;
using POC.LaunchDarkly.Shareable;
using POC.LaunchDarkly.Shareable.Behaviors;
using POC.LaunchDarkly.Shareable.Settings;

namespace POC.LaunchDarkly.IoC;

public static class AppServiceCollectionExtensions
{
	public static void ConfigureAppDependencies(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssemblies(
				typeof(IDomainEntryPoint).Assembly,
				typeof(ValidationBehavior<,>).Assembly)
		);

		services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));

		services.AddValidatorsFromAssemblyContaining<IShareableEntryPoint>();
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

		ConfigureLaunchDarkly(services, configuration);

		services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();
	}

	private static void ConfigureLaunchDarkly(IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<LaunchDarklySettings>(configuration.GetSection("LaunchDarkly"));

		services.AddSingleton<ILdClient>(sp =>
		{
			var options = sp.GetRequiredService<IOptions<LaunchDarklySettings>>().Value;
			var logger = sp.GetRequiredService<ILogger<ILdClient>>();

			var ldConfig = Configuration.Builder(options.SdkKey)
				.ServiceEndpoints(Components.ServiceEndpoints()
					.Streaming(options.StreamingUri)
					.Polling(options.PollingUri)
					.Events(options.EventsUri))
				.Build();

			var client = new LdClient(ldConfig);

			if (!client.Initialized)
			{
				logger.LogWarning("LaunchDarkly não foi inicializado corretamente. Status: {Status}", client.DataStoreStatusProvider.Status);
			}

			return client;
		});

		services.AddSingleton<Lazy<Context>>(sp =>
		{
			var options = sp.GetRequiredService<IOptions<LaunchDarklySettings>>().Value;
			return new Lazy<Context>(() => Context.New(ContextKind.Of("system"), options.EnvironmentName));
		});
	}
}
