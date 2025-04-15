using POC.LaunchDarkly.Shareable.Enums;

namespace POC.LaunchDarkly.Domain.Entities;

public class Emprestimo
{
	protected Emprestimo() 
	{
		CPF = string.Empty;
		Finalidade = string.Empty;
		NumeroConta = string.Empty;
	}

	public Emprestimo(string cpf, decimal valorSolicitado, int prazoEmMeses, string finalidade, string numeroConta) 
	{
		DataCriacao = DateTime.UtcNow;
		Status = StatusEmprestimoEnum.Pendente;
		CPF = cpf;
		ValorSolicitado = valorSolicitado;
		PrazoEmMeses = prazoEmMeses;
		Finalidade = finalidade;
		NumeroConta = numeroConta;
	}

	public Guid Id { get; }
	public DateTimeOffset DataCriacao { get; private set; }
	public StatusEmprestimoEnum Status {  get; private set; }
	public string CPF { get; private set; }
	public decimal ValorSolicitado { get; private set; } = 0;
	public int PrazoEmMeses { get; private set; }
	public string Finalidade { get; private set; }
	public string NumeroConta { get; private set; }

	public void Aprovar() => Status = StatusEmprestimoEnum.Aprovado;
	public void Recusar() => Status = StatusEmprestimoEnum.Recusado;
}