using POC.LaunchDarkly.Domain.Entities;

namespace POC.LaunchDarkly.Domain.Repositories;

public interface IEmprestimoRepository
{
	void Add(Emprestimo emprestimo);

	Task<int> SaveChanges();
}