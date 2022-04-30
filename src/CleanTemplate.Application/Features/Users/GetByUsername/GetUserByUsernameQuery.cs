using CleanTemplate.Application.Abstractions;
using CleanTemplate.Application.Extensions;
using FluentValidation;

namespace CleanTemplate.Application.Features.Users;

public class GetUserByUsernameQuery : IQuery<User>
{
	/// <example>aspadmin</example>
	public string Username { get; init; }


	public GetUserByUsernameQuery(string username)
	{
		Username = username;
	}
}

public class GetUserByUsernameQueryValidator : AbstractValidator<GetUserByUsernameQuery>
{
	public GetUserByUsernameQueryValidator()
	{
		RuleFor(o => o.Username).NotEmpty().UserName();
	}
}