using CleanTemplate.Core.Constants;
using CleanTemplate.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Data.SeedData;

public static partial class SeedExtensions
{
	public static void SeedRoles(this ModelBuilder builder)
	{
		builder.Entity<AppRole>().HasData(Roles);
		builder.Entity<AppRoleClaim>().HasData(RoleClaims);
	}

	public static readonly AppRole[] Roles =
	{
		new()
		{
			Id = 1,
			Name = "user",
			NormalizedName = "USER",
		},
		new()
		{
			Id = 2,
			Name = "admin",
			NormalizedName = "ADMIN"
		}
	};

	public static readonly AppRoleClaim[] RoleClaims =
	{
		new()
		{
			Id = 1,
			RoleId = 2,
			ClaimType = Claims.Permission,
			ClaimValue = Permissions.Admin
		}
	};
}