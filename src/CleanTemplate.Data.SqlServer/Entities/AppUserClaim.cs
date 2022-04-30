using CleanTemplate.Core.Abstractions.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanTemplate.Data.Entities;

public class AppUserClaim : IdentityUserClaim<long>, IEntity
{
	public override int Id { get; set; }
	public override long UserId { get; set; }
	public override string ClaimType { get; set; } = default!;
	public override string? ClaimValue { get; set; }


	public virtual AppUser? User { get; set; }
}

internal class AppUserClaimConfiguration : IEntityTypeConfiguration<AppUserClaim>
{
	public void Configure(EntityTypeBuilder<AppUserClaim> builder)
	{
		builder.Property(e => e.ClaimType).HasMaxLength(32);
		builder.Property(e => e.ClaimValue).HasMaxLength(128);

		builder.ToTable("AppUserClaims");
	}
}