using MediatR;
using OperationResult;
using POC.LaunchDarkly.Shareable.Responses;

namespace POC.LaunchDarkly.Shareable.Requests;

public record SimularEmprestimoRequest(
	decimal ValorSolicitado,
	int PrazoEmMeses,
	string Finalidade
) : IRequest<Result<SimularEmprestimoResponse>>;