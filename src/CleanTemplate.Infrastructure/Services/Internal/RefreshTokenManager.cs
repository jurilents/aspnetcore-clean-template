using System.Security.Cryptography;
using CleanTemplate.Application.Extensions;
using CleanTemplate.Application.Models;
using CleanTemplate.Application.Options;
using CleanTemplate.Core.Abstractions.Context;
using CleanTemplate.Core.DependencyInjection;
using CleanTemplate.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CleanTemplate.Infrastructure.Services.Internal;

[Inject]
public class RefreshTokenManager
{
	private readonly JwtOptions _options;
	private readonly IDatabaseContext _database;
	private readonly DbSet<AppRefreshToken> _refreshTokensSet;
	private readonly IHttpContextAccessor _httpContextAccessor;

	private HttpContext HttpContext => _httpContextAccessor.HttpContext!;

	public RefreshTokenManager(IOptions<JwtOptions> optionsAccessor, IHttpContextAccessor httpContextAccessor, IDatabaseContext database)
	{
		_database = database;
		_options = optionsAccessor.Value;
		_httpContextAccessor = httpContextAccessor;
		_refreshTokensSet = _database.Set<AppRefreshToken>();
	}

	public async Task<JwtToken> GenerateAsync(AppUser user, CancellationToken cancel = default)
	{
		DateTime expires = DateTime.UtcNow.Add(_options.RefreshTokenLifetime);
		string token = GenerateRandomToken();

		_refreshTokensSet.Add(new AppRefreshToken
		{
			Token = token,
			UserId = user.Id,
			IpAddress = HttpContext.GetIPAddress().ToString(),
			UserAgent = HttpContext.GetUserAgent()
		});

		await _database.SaveChangesAsync(cancel: cancel);

		return new JwtToken(token, expires);
	}

	public bool IsValid(AppRefreshToken token)
	{
		return !string.IsNullOrEmpty(token.Token)
		       && token.Created.Add(_options.RefreshTokenLifetime) > DateTime.UtcNow;
	}

	private static string GenerateRandomToken()
	{
		var randomNumber = new byte[64];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		// To base64 without ending '=='
		string base64 = Convert.ToBase64String(randomNumber)[..^2];
		return base64.Replace('/', '-').Replace('+', '_');
	}
}