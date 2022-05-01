using CleanTemplate.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace CleanTemplate.Tests.Application.SeedData;

public static class MockUsers
{
	private static readonly PasswordHasher<AppUser?> hasher = new();

	public const string Password = "TestPassword42$";
	public const string TooShortPassword = "test";
	public const string InvalidPassword = "TestPassword";

	public const string UserName = "test42";
	public const string InvalidUserName = "Invalidus667";

	public const string Email = "test42@email.test";
	public const string InvalidEmail = "invalidus667@email.test";


	public static IEnumerable<AppUser> Data => new[]
	{
		Default,
		WithoutConfirmedEmail
	};

	public static readonly AppUser Default = new()
	{
		Id = 1,
		UserName = UserName,
		NormalizedUserName = UserName.ToUpper(),
		Email = Email,
		NormalizedEmail = Email.ToUpper(),
		EmailConfirmed = true,
		PasswordHash = hasher.HashPassword(null, Password),
		SecurityStamp = "00000000-0000-0000-0000-000000000000"
	};

	public static readonly AppUser WithoutConfirmedEmail = new()
	{
		Id = 2,
		UserName = "otherUser",
		NormalizedUserName = "OTHERUSER",
		Email = "other.user@email.test",
		NormalizedEmail = "OTHER.USER@EMAIL.TEST",
		EmailConfirmed = false, // <--
		PasswordHash = hasher.HashPassword(null, Password),
		SecurityStamp = "00000000-0000-0000-0000-000000000000"
	};


	public static readonly AppUser NotExisted = new()
	{
		Id = 667,
		UserName = "invalid_test42",
		NormalizedUserName = "INVALID_TEST42",
		Email = "invalid_test42@email.test",
		NormalizedEmail = "INVALID_TEST42@EMAIL.TEST",
		PasswordHash = hasher.HashPassword(null, Password),
		SecurityStamp = "00000000-0000-0000-0000-000000000000"
	};
}