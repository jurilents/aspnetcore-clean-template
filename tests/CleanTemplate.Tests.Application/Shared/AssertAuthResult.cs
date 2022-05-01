using System.IdentityModel.Tokens.Jwt;
using CleanTemplate.Application.Features.Auth;
using CleanTemplate.Core.Abstractions.Context;
using CleanTemplate.Core.Constants;
using CleanTemplate.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CleanTemplate.Tests.Application.Shared;

public static class AssertAuthResult
{
	public static void ValidResult(AuthResult result, AppUser user)
	{
		Assert.Equal(user.UserName, result.Username);

		IsValidAccessToken(result.Token);
		Assert.True(result.TokenExpires > DateTime.UtcNow);

		IsValidRefreshToken(result.RefreshToken);
		Assert.True(result.RefreshTokenExpires > DateTime.UtcNow);
	}

	public static void IsValidAccessToken(string token)
	{
		Assert.NotEmpty(token);
		// JWT token always has 3 segments
		Assert.True(token.Split('.').Length == 3);

		var handler = new JwtSecurityTokenHandler();
		var jwtToken = handler.ReadJwtToken(token);

		Assert.Contains(jwtToken.Claims, c => c.Type == Claims.Id);
		Assert.Contains(jwtToken.Claims, c => c.Type == Claims.UserName);
	}

	public static void IsValidRefreshToken(string refreshToken)
	{
		Assert.NotEmpty(refreshToken);

		// Refresh token must be existed in DB
		// var database = TestingEnvironment.ServiceProvider.GetRequiredService<IDatabaseContext>();
		// var dbToken = database.Set<AppRefreshToken>().Find(refreshToken);
		// Assert.NotNull(dbToken);
	}
}