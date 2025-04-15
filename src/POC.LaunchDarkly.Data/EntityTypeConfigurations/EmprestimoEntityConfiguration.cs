using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POC.LaunchDarkly.Domain.Entities;

namespace POC.LaunchDarkly.Data.EntityTypeConfigurations;

public class EmprestimoEntityConfiguration : IEntityTypeConfiguration<Emprestimo>
{
	public void Configure(EntityTypeBuilder<Emprestimo> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.IsRequired()
			.ValueGeneratedOnAdd()
			.HasComment("Identificador do empréstimo");

		builder.Property(x => x.DataCriacao)
			.IsRequired()
			.HasColumnType("timestamptz")
			.HasComment("Data de criação do empréstimo");

		builder.Property(x => x.Status)
			.IsRequired()
			.HasConversion<string>()
			.HasColumnType("VARCHAR(20)")
			.HasComment("Status atual do empréstimo");

		builder.Property(x => x.CPF)
			.IsRequired()
			.HasColumnType("VARCHAR(11)")
			.HasComment("CPF do solicitante do empréstimo");

		builder.Property(x => x.ValorSolicitado)
			.IsRequired()
			.HasColumnType("DECIMAL(18,2)")
			.HasComment("Valor solicitado para o empréstimo");

		builder.Property(x => x.PrazoEmMeses)
			.IsRequired()
			.HasColumnType("INT")
			.HasComment("Prazo de pagamento em meses");

		builder.Property(x => x.Finalidade)
			.IsRequired()
			.HasColumnType("VARCHAR(100)")
			.HasComment("Finalidade declarada para o empréstimo");

		builder.Property(x => x.NumeroConta)
			.IsRequired()
			.HasMaxLength(20)
			.HasComment("Número da conta vinculada ao empréstimo");
	}
}
