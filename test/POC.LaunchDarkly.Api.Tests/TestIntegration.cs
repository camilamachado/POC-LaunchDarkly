using LaunchDarkly.Sdk.Server.Integrations;
using LaunchDarkly.Sdk.Server.Interfaces;
using LaunchDarkly.Sdk.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using POC.LaunchDarkly.Data;

namespace POC.LaunchDarkly.Api.Tests;

internal class TestIntegration : WebApplicationFactory<Program>
{
	private readonly string _enviroment;

	public TestIntegration(string enviroment = "Development")
		=> _enviroment = enviroment;

	protected override IHost CreateHost(IHostBuilder builder)
	{
		builder.ConfigureAppConfiguration((x, _) => x.Configuration["urls"] = "*");
		builder.ConfigureServices(services =>
		{
			services.AddTransient(_ => new ApplicationDbContext(
				new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("ApplicationDb").Options));

			var testData = TestData.DataSource();
			var ldConfig = Configuration.Builder("fake-sdk-key")
				.DataSource(testData)
				.Build();
			var ldClient = new LdClient(ldConfig);

			services.AddSingleton<ILdClient>(ldClient);
			services.AddSingleton(testData);
		});
		builder.UseEnvironment(_enviroment);

		return base.CreateHost(builder);
	}
}
