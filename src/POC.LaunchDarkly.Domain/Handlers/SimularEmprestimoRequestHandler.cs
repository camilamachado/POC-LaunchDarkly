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
	private readonly ILdClient _ldClient;
	private readonly Lazy<Context> _ldContext;
	private readonly ILogger<SimularEmprestimoRequestHandler> _logger;

	private decimal _taxaDeJuros = 0.05m;

	public SimularEmprestimoRequestHandler(ILdClient ldClient, Lazy<Context> ldContext, ILogger<SimularEmprestimoRequestHandler> logger)
	{
		_ldClient = ldClient;
		_ldContext = ldContext;
		_logger = logger;
	}

	public Task<Result<SimularEmprestimoResponse>> Handle(SimularEmprestimoRequest request, CancellationToken cancellationToken)
	{
		//var context = Context.Builder("premium").Build();

		if (_ldClient.BoolVariation("taxa_juros_premium", _ldContext.Value, false))
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
