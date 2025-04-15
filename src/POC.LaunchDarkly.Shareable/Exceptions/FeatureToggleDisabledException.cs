namespace POC.LaunchDarkly.Shareable.Exceptions;

public class FeatureToggleDisabledException(string message) : Exception(message);
