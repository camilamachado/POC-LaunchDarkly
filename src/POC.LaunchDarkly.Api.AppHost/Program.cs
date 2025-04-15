var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("Postgres", port: 5432)
	.WithLifetime(ContainerLifetime.Persistent)
	.WithPgAdmin(c => c.WithLifetime(ContainerLifetime.Persistent))
	.AddDatabase("Default", "csf-poc-launch-darkly-db");

builder.AddProject<Projects.POC_LaunchDarkly_Api>("poc-launchdarkly-api")
	.WithReference(postgres);

builder.Build().Run();
