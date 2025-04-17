namespace POC.LaunchDarkly.Shareable.Settings;

public record LaunchDarklySettings
{
	public string SdkKey { get; init; } = default!;
	public string RedisUri { get; init; } = default!;
	public string RedisPrefix { get; init; } = "launchdarkly";
	public string EnvironmentName { get; init; } = "default";
}