﻿using CleanTemplate.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanTemplate.Data.Configuration;

internal class AppUserClaimConfiguration : IEntityTypeConfiguration<AppUserClaim>
{
	public void Configure(EntityTypeBuilder<AppUserClaim> builder)
	{
		builder.Property(e => e.ClaimType).HasMaxLength(32);
		builder.Property(e => e.ClaimValue).HasMaxLength(128);

		builder.ToTable("AppUserClaims");
	}
}