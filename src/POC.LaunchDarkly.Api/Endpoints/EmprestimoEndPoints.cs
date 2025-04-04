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

		group.MapGet("simulacao", async ([AsParameters] SimularEmprestimoRequest request, IMediator mediator) =>
			await mediator.SendCommand(request))
			.WithName("SimularEmprestimoCallback")
			.WithDisplayName("Endpoint para simular um empréstimo")
			.Produces<SimularEmprestimoResponse>(200)
			.Produces(400);
	}
}
