using FluentValidation;
using POC.LaunchDarkly.Shareable.Requests;

namespace POC.LaunchDarkly.Shareable.Validators;

public class SimularEmprestimoValidator : AbstractValidator<SimularEmprestimoRequest>
{
	public SimularEmprestimoValidator()
	{
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
	}
}