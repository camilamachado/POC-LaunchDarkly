namespace POC.LaunchDarkly.Shareable.Responses;

public record SimularEmprestimoResponse(
	decimal ValorSolicitado,
	int PrazoEmMeses,
	decimal TaxaDeJuros,
	decimal ValorParcelaMensal,
	decimal ValorTotalAPagar
);