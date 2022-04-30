﻿using CleanTemplate.Core.Abstractions.Entities;
using Microsoft.AspNetCore.Identity;

namespace CleanTemplate.Data.Entities;

public class AppRoleClaim : IdentityRoleClaim<long>, IEntity
{
	public override int Id { get; set; }
	public override long RoleId { get; set; }
	public override string ClaimType { get; set; } = default!;
	public override string? ClaimValue { get; set; }


	public virtual AppRole? Role { get; set; }
}