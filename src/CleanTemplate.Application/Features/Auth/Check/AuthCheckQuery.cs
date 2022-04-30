using CleanTemplate.Application.Extensions;
using FluentValidation;
using MediatR;

namespace CleanTemplate.Application.Features.Auth;

public class AuthCheckQuery : IRequest<AuthCheckResult>
{
	/// <example>aspadmin</example>
	public string Login { get; init; } = default!;
}

public class AuthCheckQueryValidator : AbstractValidator<AuthCheckQuery>
{
	public AuthCheckQueryValidator()
	{
		RuleFor(o => o.Login).NotEmpty().UserName();
	}
}