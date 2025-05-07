using Flagsmith;
using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using OperationResult;
using POC.LaunchDarkly.Shareable.Requests;
using POC.LaunchDarkly.Shareable.Responses;

namespace POC.LaunchDarkly.Domain.Handlers;

public sealed class SimularEmprestimoRequestHandler : IRequestHandler<SimularEmprestimoRequest, Result<SimularEmprestimoResponse>>
{
	private readonly IFlagsmithClient _flagsmithClient;
	private readonly ILogger<SimularEmprestimoRequestHandler> _logger;

	private decimal _taxaDeJuros = 0.05m;

	public SimularEmprestimoRequestHandler(IFlagsmithClient flagsmithClient, ILogger<SimularEmprestimoRequestHandler> logger)
	{
		_flagsmithClient = flagsmithClient;
		_logger = logger;
	}

	public async Task<Result<SimularEmprestimoResponse>> Handle(SimularEmprestimoRequest request, CancellationToken cancellationToken)
	{
		var flags = await _flagsmithClient.GetEnvironmentFlags();

		if (await flags.IsFeatureEnabled("teste"))
		{
			_taxaDeJuros = 0.03m;
		}

		var valorDaParcelaMensal = (request.ValorSolicitado * (1 + _taxaDeJuros)) / request.PrazoEmMeses;
		var valorTotalAPagar = valorDaParcelaMensal * request.PrazoEmMeses;

		var simulacaoResponse = new SimularEmprestimoResponse(
			request.ValorSolicitado,
			request.PrazoEmMeses,
			_taxaDeJuros,
			valorDaParcelaMensal,
			valorTotalAPagar);

		return Result.Success(simulacaoResponse);
	}
}
