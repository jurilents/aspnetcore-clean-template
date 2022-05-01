#pragma warning disable CS8618
using CleanTemplate.Core.Abstractions.Context;
using CleanTemplate.Data.Entities;
using CleanTemplate.Data.SeedData;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Data.Context;

public class SqlServerDbContext : IdentityDbContext<AppUser, AppRole, long,
	AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>, IDatabaseContext
{
	public SqlServerDbContext(DbContextOptions options) : base(options)
	{
		// To use AsNoTracking by default
		// ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
	}


	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

		builder.SeedRoles();
		builder.SeedDefaultUser();
	}
}