using Microsoft.Extensions.Logging;

namespace POC.LaunchDarkly.Shareable.LogsSourceGenerator;

/// <summary>
/// Define os logs específicos para a funcionalidade de empréstimo.
/// </summary>
public static partial class EmprestimosLogDefinitions
{
	[LoggerMessage(
		EventId = 2001,
		Level = LogLevel.Warning,
		Message = "Iniciando simulação de empréstimo com a taxa de juros: {taxaDeJuros}%"
	)]
	public static partial void IniciandoSimulacaoEmprestimo(this ILogger logger, decimal taxaDeJuros);
}