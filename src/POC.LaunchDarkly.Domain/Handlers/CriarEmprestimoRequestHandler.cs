using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using OperationResult;
using POC.LaunchDarkly.Domain.Entities;
using POC.LaunchDarkly.Domain.Repositories;
using POC.LaunchDarkly.Shareable.Exceptions;
using POC.LaunchDarkly.Shareable.LogsSourceGenerator;
using POC.LaunchDarkly.Shareable.Requests;
using POC.LaunchDarkly.Shareable.Responses;

namespace POC.LaunchDarkly.Domain.Handlers;

public sealed class CriarEmprestimoRequestHandler : IRequestHandler<CriarEmprestimoRequest, Result<CriarEmprestimoResponse>>
{
	private readonly ILdClient _ldClient;
	private readonly Lazy<Context> _ldContext;
	private readonly IEmprestimoRepository _emprestimoRepository;
	private readonly ILogger<SimularEmprestimoRequestHandler> _logger;

	public CriarEmprestimoRequestHandler(ILdClient ldClient, Lazy<Context> ldContext, IEmprestimoRepository emprestimoRepository, ILogger<SimularEmprestimoRequestHandler> logger)
	{
		_ldClient = ldClient;
		_ldContext = ldContext;
		_emprestimoRepository = emprestimoRepository;
		_logger = logger;
	}

	public async Task<Result<CriarEmprestimoResponse>> Handle(CriarEmprestimoRequest request, CancellationToken cancellationToken)
	{
		if(_ldClient.BoolVariation("habilitar_emprestimos", _ldContext.Value, false))
		{
			var emprestimo = new Emprestimo(request.CPF, request.ValorSolicitado, request.PrazoEmMeses, request.Finalidade, request.NumeroConta);

			_emprestimoRepository.Add(emprestimo);
			await _emprestimoRepository.SaveChanges();

			return new CriarEmprestimoResponse(emprestimo.Id);
		}

		_logger.FeatureDesativada("habilitar_emprestimos");

		return new FeatureToggleDisabledException("A funcionalidade de criação de empréstimos está temporariamente desativada.");
	}
}