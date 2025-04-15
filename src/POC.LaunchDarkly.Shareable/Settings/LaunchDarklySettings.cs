namespace POC.LaunchDarkly.Shareable.Settings;

public record LaunchDarklySettings
{
	public string SdkKey { get; init; } = default!;
	public string StreamingUri { get; init; } = "https://stream.launchdarkly.com";
	public string PollingUri { get; init; } = "https://sdk.launchdarkly.com";
	public string EventsUri { get; init; } = "https://events.launchdarkly.com";
	public string EnvironmentName { get; init; } = "default";
}