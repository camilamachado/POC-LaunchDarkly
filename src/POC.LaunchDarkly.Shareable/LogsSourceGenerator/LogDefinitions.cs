using Microsoft.Extensions.Logging;

namespace POC.LaunchDarkly.Shareable.LogsSourceGenerator;

/// <summary>
/// Define os logs reutilizáveis e logs que fazem parte do fluxo geral da aplicação.
/// </summary>
public static partial class LogDefinitions
{
	[LoggerMessage(
		EventId = 1001,
		Level = LogLevel.Warning,
		Message = "Feature '{feature}' está temporariamente desativada"
	)]
	public static partial void FeatureDesativada(this ILogger logger, string feature);

	[LoggerMessage(
		EventId = 1002,
		Level = LogLevel.Error,
		Message = "Erro inesperado"
	)]
	public static partial void ErroInesperado(this ILogger logger, Exception exception);
}