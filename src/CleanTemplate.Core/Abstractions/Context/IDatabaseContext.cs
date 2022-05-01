using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CleanTemplate.Core.Abstractions.Context;

public interface IDatabaseContext
{
	DatabaseFacade Database { get; }
	DbSet<TEntity> Set<TEntity>() where TEntity : class;
	EntityEntry Entry(object entity);
	Task<int> SaveChangesAsync(CancellationToken cancel = default);
	int SaveChanges();
}