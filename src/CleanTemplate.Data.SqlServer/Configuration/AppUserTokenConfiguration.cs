using CleanTemplate.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanTemplate.Data.Configuration;

internal class AppUserTokenConfiguration : IEntityTypeConfiguration<AppUserToken>
{
	public void Configure(EntityTypeBuilder<AppUserToken> builder)
	{
		builder.Property(e => e.LoginProvider).HasMaxLength(64);
		builder.Property(e => e.Name).HasMaxLength(128);
		builder.Property(e => e.Value).HasMaxLength(256);

		builder.ToTable("AppUserTokens");
	}
}