﻿using CleanTemplate.Core.Abstractions.Entities;
using Microsoft.AspNetCore.Identity;

namespace CleanTemplate.Data.Entities;

public class AppUserClaim : IdentityUserClaim<long>, IEntity
{
	public override int Id { get; set; }
	public override long UserId { get; set; }
	public override string ClaimType { get; set; } = default!;
	public override string? ClaimValue { get; set; }


	public virtual AppUser? User { get; set; }
}