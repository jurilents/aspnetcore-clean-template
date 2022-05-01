using CleanTemplate.Application.Features.Auth;
using CleanTemplate.Core.Enums;
using CleanTemplate.Tests.Application.SeedData;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CleanTemplate.Tests.Application.Features.Auth;

public class AuthCheckHandlerTests
{
	private readonly IMediator _mediator;

	public AuthCheckHandlerTests()
	{
		_mediator = TestingEnvironment.ServiceProvider.GetRequiredService<IMediator>();
	}


	[Fact]
	public async Task Check_ByUsername_With_ExistedUser()
	{
		var query = new AuthCheckQuery { Login = MockUsers.Default.UserName };
		var result = await _mediator.Send(query);

		AssetsCheckWithExistedUserResult(result);
	}

	[Fact]
	public async Task Check_ByEmail_With_ExistedUser()
	{
		var query = new AuthCheckQuery { Login = MockUsers.Default.Email };
		var result = await _mediator.Send(query);

		AssetsCheckWithExistedUserResult(result);
	}

	[Fact]
	public async Task Check_ByUsername_With_Not_ExistedUser()
	{
		var query = new AuthCheckQuery { Login = MockUsers.NotExisted.UserName };
		var result = await _mediator.Send(query);

		AssetsCheckWithNotExistedUserResult(result);
	}

	[Fact]
	public async Task Check_ByEmail_With_Not_ExistedUser()
	{
		var query = new AuthCheckQuery { Login = MockUsers.NotExisted.Email };
		var result = await _mediator.Send(query);

		AssetsCheckWithNotExistedUserResult(result);
	}


	private static void AssetsCheckWithExistedUserResult(AuthCheckResult result)
	{
		Assert.True(result.UserExists);
	}

	private static void AssetsCheckWithNotExistedUserResult(AuthCheckResult result)
	{
		Assert.False(result.UserExists);
		Assert.Equal(expected: AuthMethod.Register, actual: result.PreferAuthMethod);
		Assert.Single(result.AllowedAuthMethod);
		Assert.Equal(expected: AuthMethod.Register, actual: result.AllowedAuthMethod.First());
	}
}