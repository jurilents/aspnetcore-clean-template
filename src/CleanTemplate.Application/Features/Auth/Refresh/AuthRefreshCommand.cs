using FluentValidation;
using MediatR;

namespace CleanTemplate.Application.Features.Auth;

public class AuthRefreshCommand : IRequest<AuthResult>
{
	/// <example>[Base64]</example>
	public string RefreshToken { get; init; } = default!;
}

public class AuthRefreshCommandValidator : AbstractValidator<AuthRefreshCommand>
{
	public AuthRefreshCommandValidator()
	{
		RuleFor(o => o.RefreshToken).NotEmpty();
	}
}