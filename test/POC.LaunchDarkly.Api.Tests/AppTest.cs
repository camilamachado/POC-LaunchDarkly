using FluentAssertions;
using LaunchDarkly.Sdk.Server.Integrations;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using POC.LaunchDarkly.Shareable.Requests;
using POC.LaunchDarkly.Shareable.Responses;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace POC.LaunchDarkly.Api.Tests;

public class AppTest
{
	private static readonly TestIntegration _application = new ();
	private static readonly HttpClient _httpClient = _application.CreateClient();
	private static readonly TestData _launchDarklyTestData = _application.Services.GetRequiredService<TestData>();

	[Fact]
	public async Task Dado_FeatureToggleAtivado_Quando_CriarEmprestimo_Entao_CriaComSucesso()
	{
		// Arrange
		var request = new CriarEmprestimoRequest("12345678900", 1000m, 12, "Viagem", "1234");
		_launchDarklyTestData.Update(_launchDarklyTestData.Flag("habilitar_emprestimos").VariationForAll(true));

		// Act
		var response = await _httpClient.PostAsJsonAsync("/api/v1/emprestimos", request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var responseBody = await response.Content.ReadAsStringAsync();
		var emprestimoCriado = JsonSerializer.Deserialize<CriarEmprestimoResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
		emprestimoCriado.IdentificadorEmprestimo.Should().NotBeEmpty();
	}

	[Fact]
	public async Task Dado_FeatureToggleDesativado_Quando_CriarEmprestimo_Entao_RetornaServicoNaoDisponivel()
	{
		// Arrange
		var request = new CriarEmprestimoRequest("12345678900", 1000m, 12, "Viagem", "1234");
		_launchDarklyTestData.Update(_launchDarklyTestData.Flag("habilitar_emprestimos").VariationForAll(false));

		// Act
		var response = await _httpClient.PostAsJsonAsync("/api/v1/emprestimos", request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);

		var responseBody = await response.Content.ReadAsStringAsync();
		responseBody.Should().Contain("A funcionalidade de criação de empréstimos está temporariamente desativada.");
	}

	[Fact]
	public async Task Dado_FeatureToggleAtivado_Quando_SimularEmprestimo_Entao_AplicaTaxaDeJurosPremium()
	{
		// Arrange
		var request = new SimularEmprestimoRequest(10000m, 10, "Viagem");
		_launchDarklyTestData.Update(_launchDarklyTestData.Flag("taxa_juros_premium").VariationForAll(true));

		var url = QueryHelpers.AddQueryString("/api/v1/emprestimos/simulacao", new Dictionary<string, string>
		{
			["ValorSolicitado"] = request.ValorSolicitado.ToString(),
			["PrazoEmMeses"] = request.PrazoEmMeses.ToString(),
			["Finalidade"] = request.Finalidade
		});

		// Act
		var response = await _httpClient.GetAsync(url);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var responseBody = await response.Content.ReadAsStringAsync();
		var simulacao = JsonSerializer.Deserialize<SimularEmprestimoResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

		simulacao.Should().NotBeNull();
		simulacao!.TaxaDeJuros.Should().Be(0.03m);
		simulacao.ValorParcelaMensal.Should().Be(1030m);
		simulacao.ValorTotalAPagar.Should().Be(10300m);
	}

	[Fact]
	public async Task Dado_FeatureToggleDesativado_Quando_SimularEmprestimo_Entao_AplicaTaxaDeJurosPadrao()
	{
		// Arrange
		var request = new SimularEmprestimoRequest(10000m, 10, "Viagem");
		_launchDarklyTestData.Update(_launchDarklyTestData.Flag("taxa_juros_premium").VariationForAll(false));

		var url = QueryHelpers.AddQueryString("/api/v1/emprestimos/simulacao", new Dictionary<string, string>
		{
			["ValorSolicitado"] = request.ValorSolicitado.ToString(),
			["PrazoEmMeses"] = request.PrazoEmMeses.ToString(),
			["Finalidade"] = request.Finalidade
		});

		// Act
		var response = await _httpClient.GetAsync(url);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var responseBody = await response.Content.ReadAsStringAsync();
		var simulacao = JsonSerializer.Deserialize<SimularEmprestimoResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

		simulacao.Should().NotBeNull();
		simulacao!.TaxaDeJuros.Should().Be(0.05m);
		simulacao.ValorParcelaMensal.Should().Be(1050m);
		simulacao.ValorTotalAPagar.Should().Be(10500m);
	}
}
