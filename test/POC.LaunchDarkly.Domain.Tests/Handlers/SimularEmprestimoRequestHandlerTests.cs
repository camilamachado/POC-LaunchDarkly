using FluentAssertions;
using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;
using POC.LaunchDarkly.Domain.Handlers;
using POC.LaunchDarkly.Shareable.Requests;

namespace POC.LaunchDarkly.Domain.Tests.Handlers;

public class SimularEmprestimoRequestHandlerTests
{
	private readonly ILdClient _ldClient = Substitute.For<ILdClient>();
	private readonly Lazy<Context> _context;
	private readonly ILogger<SimularEmprestimoRequestHandler> _logger = Substitute.For<ILogger<SimularEmprestimoRequestHandler>>();
	private readonly SimularEmprestimoRequestHandler _sut;

	public SimularEmprestimoRequestHandlerTests()
	{
		_context = new Lazy<Context>(() => Context.New("test-poc-launch-darkly-api"));
		_sut = new SimularEmprestimoRequestHandler(_ldClient, _context, _logger);
	}

	[Fact]
	public async Task Dado_FeatureToggleAtivado_Quando_SimularEmprestimo_Entao_AplicaTaxaDeJurosPremium()
	{
		// Arrange
		var request = new SimularEmprestimoRequest(10000m, 10, "Viagem");
		_ldClient.BoolVariation("taxa_juros_premium", Arg.Any<Context>(), Arg.Any<bool>()).Returns(true);

		// Act
		var (sucesso, resultado, erro) = await _sut.Handle(request, CancellationToken.None);

		// Assert
		sucesso.Should().BeTrue();

		resultado.Should().NotBeNull();
		resultado!.TaxaDeJuros.Should().Be(0.03m);
		resultado.ValorParcelaMensal.Should().BeApproximately(1030m, 0.01m);
		resultado.ValorTotalAPagar.Should().BeApproximately(10300m, 0.01m);
	}

	[Fact]
	public async Task Dado_FeatureToggleDesativado_Quando_SimularEmprestimo_Entao_AplicaTaxaDeJurosPadrao()
	{
		// Arrange
		var request = new SimularEmprestimoRequest(10000m, 10, "Viagem");
		_ldClient.BoolVariation("taxa_juros_premium", Arg.Any<Context>(), Arg.Any<bool>()).Returns(false);

		// Act
		var (sucesso, resultado, erro) = await _sut.Handle(request, CancellationToken.None);

		// Assert
		sucesso.Should().BeTrue();
		erro.Should().BeNull();

		resultado.Should().NotBeNull();
		resultado!.TaxaDeJuros.Should().Be(0.05m);
		resultado.ValorParcelaMensal.Should().BeApproximately(1050m, 0.01m);
		resultado.ValorTotalAPagar.Should().BeApproximately(10500m, 0.01m);
	}
}
