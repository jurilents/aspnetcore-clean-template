using CleanTemplate.Application.Features.Auth;
using CleanTemplate.Core.Exceptions;
using CleanTemplate.Tests.Application.SeedData;
using CleanTemplate.Tests.Application.Shared;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CleanTemplate.Tests.Application.Features.Auth;

public class AuthCompleteHandlerTests
{
	private readonly IMediator _mediator;

	public AuthCompleteHandlerTests()
	{
		_mediator = TestingEnvironment.ServiceProvider.GetRequiredService<IMediator>();
	}

	[Fact]
	public async Task Login_ByUserName_For_ExistedUser_With_ValidPassword()
	{
		var command = new AuthCompleteCommand { Login = MockUsers.Default.UserName, Password = MockUsers.Password };
		AuthResult result = await _mediator.Send(command);
		AssertAuthResult.ValidResult(result, MockUsers.Default);
	}

	[Fact]
	public async Task Login_ByEmail_For_ExistedUser_With_ValidPassword()
	{
		var command = new AuthCompleteCommand { Login = MockUsers.Default.Email, Password = MockUsers.Password };
		AuthResult result = await _mediator.Send(command);
		AssertAuthResult.ValidResult(result, MockUsers.Default);
	}

	[Fact]
	public async Task Login_With_InvalidPassword_Throws_ValidationFailedException()
	{
		var command = new AuthCompleteCommand { Login = MockUsers.UserName, Password = MockUsers.InvalidPassword };
		await Assert.ThrowsAsync<ValidationFailedException>(async () => await _mediator.Send(command));
	}

	[Fact]
	public async Task Login_For_NotExistedUser_Throws_NotFoundException()
	{
		var command = new AuthCompleteCommand { Login = MockUsers.InvalidUserName, Password = MockUsers.Password };
		await Assert.ThrowsAsync<NotFoundException>(async () => await _mediator.Send(command));
	}
}