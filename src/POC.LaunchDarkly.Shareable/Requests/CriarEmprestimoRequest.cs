using MediatR;
using OperationResult;
using POC.LaunchDarkly.Shareable.Responses;
using POC.LaunchDarkly.Shareable.Validators;

namespace POC.LaunchDarkly.Shareable.Requests;

public record CriarEmprestimoRequest(
	string CPF,
	decimal ValorSolicitado,
	int PrazoEmMeses,
	string Finalidade,
	string NumeroConta
) : IRequest<Result<CriarEmprestimoResponse>>, IValidatableRequest;