using Microsoft.EntityFrameworkCore;
using POC.LaunchDarkly.Domain.Entities;

namespace POC.LaunchDarkly.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<Emprestimo> Emprestimo { get; set; } = default!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
		=> modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
		=> configurationBuilder
			.Properties<string>()
			.AreUnicode(false)
			.HaveMaxLength(255);
}