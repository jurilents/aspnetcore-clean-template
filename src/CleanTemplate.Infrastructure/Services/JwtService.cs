using CleanTemplate.Application.Features.Auth;
using CleanTemplate.Application.Services;
using CleanTemplate.Core.Abstractions.Context;
using CleanTemplate.Core.DependencyInjection;
using CleanTemplate.Core.Exceptions;
using CleanTemplate.Data.Entities;
using CleanTemplate.Data.Extensions;
using CleanTemplate.Infrastructure.Services.Internal;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Infrastructure.Services;

[Inject]
public class JwtService : IJwtService
{
	private readonly IDatabaseContext _database;
	private readonly RefreshTokenGenerator _refreshTokenGenerator;
	private readonly AccessTokenGenerator _accessTokenGenerator;

	public JwtService(IDatabaseContext database, AccessTokenGenerator accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator)
	{
		_database = database;
		_refreshTokenGenerator = refreshTokenGenerator;
		_accessTokenGenerator = accessTokenGenerator;
	}

	public async Task<AuthResult> GenerateAsync(AppUser user, CancellationToken cancel)
	{
		if (user is null) throw new InternalServerException($"Invalid argument '{nameof(user)}'");

		(string? accessToken, DateTime accessTokenExpires) = await _accessTokenGenerator.GenerateAsync(user, cancel);
		(string? refreshToken, DateTime refreshTokenExpires) = await _refreshTokenGenerator.GenerateAsync(user, cancel);

		return new AuthResult
		{
			Username = user.UserName,
			Token = accessToken,
			TokenExpires = accessTokenExpires,
			RefreshToken = refreshToken,
			RefreshTokenExpires = refreshTokenExpires
		};
	}

	public async Task<AuthResult> RefreshAsync(string refreshToken, CancellationToken cancel)
	{
		var refreshTokens = _database.Set<AppRefreshToken>();

		var token = await refreshTokens.Where(rt => rt.Token == refreshToken).Include(rt => rt.User).FirstOr404Async(cancel);
		if (!_refreshTokenGenerator.IsValid(token))
			throw new ValidationFailedException("Refresh token is invalid.");

		var result = await GenerateAsync(token.User!, cancel);
		token.User = null;

		refreshTokens.Remove(token);
		await _database.SaveChangesAsync(cancel: cancel);

		return result;
	}
}