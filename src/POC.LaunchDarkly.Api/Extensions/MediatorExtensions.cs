using MediatR;
using OperationResult;

namespace POC.LaunchDarkly.Api.Extensions;

public static class MediatorExtensions
{
	public static async Task<IResult> SendCommand<TResponse>(
		this IMediator mediator,
		IRequest<Result<TResponse>> request)
	{
		var result = await mediator.Send(request);

		if (!result.IsSuccess)
		{
			throw result.Exception;
		}

		return Results.Ok(result.Value);
	}
}
