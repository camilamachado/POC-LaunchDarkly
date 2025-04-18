﻿using MediatR;
using OperationResult;
using POC.LaunchDarkly.Shareable.Responses;
using POC.LaunchDarkly.Shareable.Validators;

namespace POC.LaunchDarkly.Shareable.Requests;

public record SimularEmprestimoRequest(
	decimal ValorSolicitado,
	int PrazoEmMeses,
	string Finalidade
) : IRequest<Result<SimularEmprestimoResponse>>, IValidatableRequest;