using MediatR;
using POC.LaunchDarkly.Api.Extensions;
using POC.LaunchDarkly.Shareable.Requests;
using POC.LaunchDarkly.Shareable.Responses;

namespace POC.LaunchDarkly.Api.Endpoints;

public static class EmprestimoEndPoints
{ 
	public static void MapEmprestimoEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/v1/emprestimos")
						.WithTags("Emprestimo");

		group.MapGet("simulacao", async ([AsParameters] SimularEmprestimoRequest request, IMediator mediator) 
			=> await mediator.SendCommand(request))
				.WithName("SimularEmprestimoCallback")
				.WithDisplayName("Endpoint para simular um empréstimo")
				.WithSummary("Simular Empréstimo")
				.WithDescription("Endpoint para simular um empréstimo com base nos dados fornecidos.")
				.Produces<SimularEmprestimoResponse>(200);

		group.MapPost("", static async (CriarEmprestimoRequest request, IMediator mediator)
			=> await mediator.SendCommand(request))
				.WithName("CriarEmprestimoCallback")
				.WithDisplayName("Endpoint para receber callback de criar empréstimo")
				.WithSummary("Criar Empréstimo")
				.WithDescription("Endpoint para receber a solicitação de criação de um empréstimo.")
				.WithTags("Emprestimo")
				.Produces<CriarEmprestimoResponse>(200);
	}
}
