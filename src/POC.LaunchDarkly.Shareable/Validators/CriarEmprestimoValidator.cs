using FluentValidation;
using POC.LaunchDarkly.Shareable.Requests;

namespace POC.LaunchDarkly.Shareable.Validators;

public class CriarEmprestimoValidator : AbstractValidator<CriarEmprestimoRequest>
{
	public CriarEmprestimoValidator()
	{
		RuleFor(x => x.CPF)
			.NotEmpty()
			.Length(11);

		RuleFor(x => x.ValorSolicitado)
			.NotNull()
			.GreaterThan(0)
			.LessThanOrEqualTo(decimal.MaxValue);

		RuleFor(x => x.PrazoEmMeses)
			.NotNull()
			.GreaterThan(0)
			.LessThanOrEqualTo(120);

		RuleFor(x => x.Finalidade)
			.MaximumLength(100);

		RuleFor(x => x.NumeroConta)
			.NotEmpty()
			.MinimumLength(1)
			.MaximumLength(20);
	}
}