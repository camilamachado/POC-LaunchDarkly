using POC.LaunchDarkly.Domain.Entities;
using POC.LaunchDarkly.Domain.Repositories;

namespace POC.LaunchDarkly.Data.Repositories;

public class EmprestimoRepository : IEmprestimoRepository
{
	private readonly ApplicationDbContext _dbContext;

	public EmprestimoRepository(ApplicationDbContext context)
	{
		_dbContext = context;
	}

	public void Add(Emprestimo emprestimo)
	{
		_dbContext.Add(emprestimo);
	}

	public async Task<int> SaveChanges()
	{
		return await _dbContext.SaveChangesAsync();
	}
}
