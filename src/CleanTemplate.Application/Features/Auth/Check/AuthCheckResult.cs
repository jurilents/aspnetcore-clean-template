using CleanTemplate.Core.Enums;

namespace CleanTemplate.Application.Features.Auth;

public record AuthCheckResult(
	bool UserExists,
	string? Username,
	AuthMethod PreferAuthMethod,
	IEnumerable<AuthMethod> AllowedAuthMethod
);