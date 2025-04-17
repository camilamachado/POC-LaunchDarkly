var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("Postgres", port: 5432)
				.WithLifetime(ContainerLifetime.Persistent)
				.WithPgAdmin(c => c.WithLifetime(ContainerLifetime.Persistent))
				.AddDatabase("Default", "csf-poc-launch-darkly-db");

var redis = builder.AddRedis("redis", port: 6379)
				.WithLifetime(ContainerLifetime.Persistent);

var ldRelay = builder.AddContainer("ld-relay", "launchdarkly/ld-relay")
				.WithEnvironment("STREAM_URI", "https://stream.launchdarkly.com")
				.WithEnvironment("BASE_URI", "https://app.launchdarkly.com")
				.WithEnvironment("EVENTS_HOST", "https://events.launchdarkly.com")
				.WithEnvironment("LD_ENV_test", "sdk-02c8c6c3-c525-4d7f-81c7-90355ec78ca9")
				.WithEnvironment("LD_PREFIX_test", "launchdarkly")
				.WithEnvironment("USE_REDIS", "true")
				.WithEnvironment("REDIS_HOST", "redis")
				.WithEnvironment("REDIS_PORT", "6379")
				.WithEnvironment("REDIS_PASSWORD", "dyXfFj9wArGXFWz7Uff4vd")
				.WithEnvironment("REDIS_LOCAL_TTL", "30")
				.WithHttpEndpoint(8030, 8030)
				.WithLifetime(ContainerLifetime.Persistent)
				.WithReference(redis);

builder.AddProject<Projects.POC_LaunchDarkly_Api>("poc-launchdarkly-api")
	.WithReference(postgres)
	.WaitFor(postgres)
	.WaitFor(ldRelay);

builder.Build().Run();
