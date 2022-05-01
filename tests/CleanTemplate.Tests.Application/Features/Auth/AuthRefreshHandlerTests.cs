using CleanTemplate.Application.Features.Auth;
using CleanTemplate.Application.Services;
using CleanTemplate.Core.Exceptions;
using CleanTemplate.Tests.Application.SeedData;
using CleanTemplate.Tests.Application.Shared;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CleanTemplate.Tests.Application.Features.Auth;

public class AuthRefreshHandlerTests
{
	private readonly IMediator _mediator;
	private readonly IJwtService _jwtService;

	public AuthRefreshHandlerTests()
	{
		_mediator = TestingEnvironment.ServiceProvider.GetRequiredService<IMediator>();
		_jwtService = TestingEnvironment.ServiceProvider.GetRequiredService<IJwtService>();
	}

	[Fact]
	public async Task Refresh_With_ValidRefreshToken()
	{
		var user = MockUsers.Default;
		var jwt = await _jwtService.GenerateAsync(user);
		var command = new AuthRefreshCommand { RefreshToken = jwt.RefreshToken };
		AuthResult result = await _mediator.Send(command);
		AssertAuthResult.ValidResult(result, MockUsers.Default);
	}

	[Fact]
	public async Task Refresh_With_InvalidRefreshToken()
	{
		var command = new AuthRefreshCommand { RefreshToken = "invalid_token" };
		await Assert.ThrowsAsync<NotFoundException>(async () => await _mediator.Send(command));
	}
}