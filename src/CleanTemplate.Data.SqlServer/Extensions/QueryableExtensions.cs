using System.Linq.Dynamic.Core;
using CleanTemplate.Core.Abstractions.Entities;
using CleanTemplate.Core.Exceptions;
using CleanTemplate.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Data.Extensions;

public static class QueryableExtensions
{
	public static async Task<TEntity> FirstOr404Async<TEntity>(this IQueryable<TEntity> queryable, CancellationToken cancel = default)
			where TEntity : class, IEntity
	{
		return await queryable.FirstOrDefaultAsync(cancel)
		       ?? throw new NotFoundException(typeof(TEntity).Name.CamelCaseToWords() + " not found.");
	}

	public static IQueryable<TEntity> Filter<TEntity>(this IQueryable<TEntity> queryable, string? filter, string? sorting, int? skip, int? take)
			where TEntity : class, IEntity
	{
		return queryable
				.AsNoTracking()
				.ApplyFiltering(filter)
				.ApplyPaging(skip, take)
				.ApplySorting(sorting);
	}

	public static IQueryable<TEntity> ApplyFiltering<TEntity>(this IQueryable<TEntity> queryable, string? filter)
			where TEntity : class, IEntity
	{
		if (string.IsNullOrEmpty(filter))
			return queryable;

		return queryable.Where(filter);
	}

	public static IQueryable<TEntity> ApplySorting<TEntity>(this IQueryable<TEntity> queryable, string? sorting)
			where TEntity : class, IEntity
	{
		if (string.IsNullOrEmpty(sorting))
			return queryable;

		foreach (string s in sorting.Split(',')) 
			queryable = queryable.OrderBy(s);
		return queryable;
	}

	public static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> queryable, int? skip, int? take)
			where TEntity : class, IEntity
	{
		if (skip is not null && take is not null)
			return queryable.Skip((int) skip).Take((int) take);

		return queryable;
	}

	public static IQueryable<TEntity> IncludeMany<TEntity>(this IQueryable<TEntity> queryable, params string[] inclusions)
			where TEntity : class, IEntity
	{
		if (inclusions.Length <= 0)
			return queryable;

		foreach (string inclusion in inclusions)
			queryable = queryable.Include(inclusion);
		return queryable;
	}
}