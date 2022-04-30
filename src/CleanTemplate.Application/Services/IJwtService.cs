using CleanTemplate.Application.Features.Auth;
using CleanTemplate.Data.Entities;

namespace CleanTemplate.Application.Services;

public interface IJwtService
{
	Task<AuthResult> GenerateAsync(AppUser user, CancellationToken cancel = default);
	Task<AuthResult> RefreshAsync(string refreshToken, CancellationToken cancel = default);
}