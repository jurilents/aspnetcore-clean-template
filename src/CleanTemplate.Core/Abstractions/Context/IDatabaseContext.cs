using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanTemplate.Core.Abstractions.Context;

public interface IDatabaseContext
{
	DbSet<TEntity> Set<TEntity>() where TEntity : class;
	EntityEntry Entry(object entity);
	Task<int> SaveChangesAsync(CancellationToken cancel = default);
	int SaveChanges();
}