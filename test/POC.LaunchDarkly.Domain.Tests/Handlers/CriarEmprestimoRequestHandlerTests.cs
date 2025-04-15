using FluentAssertions;
using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;
using POC.LaunchDarkly.Domain.Entities;
using POC.LaunchDarkly.Domain.Handlers;
using POC.LaunchDarkly.Domain.Repositories;
using POC.LaunchDarkly.Shareable.Exceptions;
using POC.LaunchDarkly.Shareable.Requests;
using POC.LaunchDarkly.Shareable.Responses;

namespace POC.LaunchDarkly.Domain.Tests.Handlers;

public class CriarEmprestimoRequestHandlerTests
{
	private readonly ILdClient _ldClient = Substitute.For<ILdClient>();
	private readonly Lazy<Context> _context;
	private readonly IEmprestimoRepository _emprestimoRepository = Substitute.For<IEmprestimoRepository>();
	private readonly ILogger<SimularEmprestimoRequestHandler> _logger = Substitute.For<ILogger<SimularEmprestimoRequestHandler>>();
	private readonly CriarEmprestimoRequestHandler _sut;

	public CriarEmprestimoRequestHandlerTests()
	{
		_context = new Lazy<Context>(() => Context.New("test-poc-launch-darkly-api"));
		_sut = new CriarEmprestimoRequestHandler(_ldClient, _context, _emprestimoRepository, _logger);
	}

	[Fact]
	public async Task Dado_FeatureToggleAtivado_Quando_CriarEmprestimo_Entao_CriaComSucesso()
	{
		// Arrange
		var emprestimo = new Emprestimo("12345678900", 1000m, 12, "Viagem", "1234");
		var request = new CriarEmprestimoRequest("12345678900", 1000m, 12, "Viagem", "1234");

		_ldClient.BoolVariation("habilitar_emprestimos", Arg.Any<Context>(), Arg.Any<bool>()).Returns(true);
		_emprestimoRepository.Add(emprestimo);
		_emprestimoRepository.SaveChanges().Returns(Task.FromResult(1));

		// Act
		var (sucesso, resultado, _) = await _sut.Handle(request, CancellationToken.None);

		// Assert
		sucesso.Should().BeTrue();
		resultado.Should().NotBeNull();
		resultado.Should().BeOfType<CriarEmprestimoResponse>();

		_emprestimoRepository.Received(1).Add(emprestimo);
		await _emprestimoRepository.Received(1).SaveChanges();
	}

	[Fact]
	public async Task Dado_FeatureToggleDesativado_Quando_CriarEmprestimo_Entao_RetornaErro()
	{
		// Arrange
		var request = new CriarEmprestimoRequest("12345678900", 1000m, 12, "Viagem", "1234");
		_ldClient.BoolVariation("habilitar_emprestimos", Arg.Any<Context>(), Arg.Any<bool>()).Returns(false);

		// Act
		var (sucesso, _, erro) = await _sut.Handle(request, CancellationToken.None);

		// Assert
		sucesso.Should().BeFalse();
		erro.Should().NotBeNull();
		erro.Should().BeOfType<FeatureToggleDisabledException>();
		erro!.Message.Should().Be("A funcionalidade de criação de empréstimos está temporariamente desativada.");

		_emprestimoRepository.DidNotReceive().Add(Arg.Any<Emprestimo>());
		await _emprestimoRepository.DidNotReceive().SaveChanges();
	}
}
