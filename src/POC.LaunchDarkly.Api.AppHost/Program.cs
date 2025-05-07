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

// Edge Proxy (sem config.json, via ENVIRONMENT_KEY_PAIRS)
var edgeProxy = builder.AddContainer("flagsmith-edge-proxy", "flagsmith/edge-proxy:latest")
	.WithEnvironment("ENVIRONMENT_KEY_PAIRS", "[{\"server_side_key\":\"ser.gtE5DtCLnCa72nPE2PTLa7\",\"client_side_key\":\"c9534NGr88eobmnk55zPHD\"},{\"server_side_key\":\"ser.nxTMfZnYnVhXPGNGC9XYr8\",\"client_side_key\":\"H9fULMxPeg6Y6vWuQGaeJg\"}]")
	.WithEnvironment("API_URL", "http://flagsmith:8000/api/v1")
	.WithEnvironment("API_POLL_FREQUENCY_SECONDS", "60")
	.WithEnvironment("LOGGING", "{\"log_level\":\"DEBUG\",\"log_format\":\"json\"}")
	.WithHttpEndpoint(8001, 8000)
	.WaitFor(flagsmith)
	.WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<Projects.POC_LaunchDarkly_Api>("poc-launchdarkly-api")
	.WithReference(postgres)
	.WaitFor(postgres)
	.WaitFor(edgeProxy);

builder.Build().Run();
