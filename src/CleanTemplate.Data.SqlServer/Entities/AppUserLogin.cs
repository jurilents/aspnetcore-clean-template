using CleanTemplate.Core.Abstractions.Entities;
using Microsoft.AspNetCore.Identity;

namespace CleanTemplate.Data.Entities;

public class AppUserLogin : IdentityUserLogin<long>, IEntity
{
	public override long UserId { get; set; }
	public override string LoginProvider { get; set; } = default!;
	public override string ProviderKey { get; set; } = default!;
	public override string? ProviderDisplayName { get; set; }

	public virtual AppUser? User { get; set; }
}