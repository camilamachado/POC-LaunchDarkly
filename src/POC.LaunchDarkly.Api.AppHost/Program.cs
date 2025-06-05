using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("Postgres", port: 5432)
				.WithLifetime(ContainerLifetime.Persistent)
				.WithPgAdmin(c => c.WithLifetime(ContainerLifetime.Persistent))
				.AddDatabase("Default", "csf-poc-launch-darkly-db");

var flagsmith = builder.AddContainer("flagsmith", "docker.flagsmith.com/flagsmith/flagsmith:latest")
	.WithEnvironment("DATABASE_URL", "postgresql://postgres:(WTJPu{MTQmM+hjmwW8+bu@postgres:5432/flagsmith-db")
	.WithEnvironment("USE_POSTGRES_FOR_ANALYTICS", "true")
	.WithEnvironment("ENVIRONMENT", "production")
	.WithEnvironment("DJANGO_ALLOWED_HOSTS", "*")
	.WithEnvironment("FLAGSMITH_DOMAIN", "localhost:8000")
	.WithEnvironment("DJANGO_SECRET_KEY", "secret")
	.WithEnvironment("PROMETHEUS_ENABLED", "true")
	.WithHttpEndpoint(8000, 8000)
	.WithReference(postgres)
	.WaitFor(postgres)
	.WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<Projects.POC_LaunchDarkly_Api>("poc-launchdarkly-api")
	.WithReference(postgres)
	.WaitFor(postgres)
	.WaitFor(flagsmith);

builder.Build().Run();
