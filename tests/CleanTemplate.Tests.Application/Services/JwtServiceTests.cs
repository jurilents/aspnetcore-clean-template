using CleanTemplate.Application.Services;
using CleanTemplate.Tests.Application.SeedData;
using CleanTemplate.Tests.Application.Shared;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CleanTemplate.Tests.Application.Services;

public class JwtServiceTests
{
	private readonly IJwtService _jwtService;

	public JwtServiceTests()
	{
		_jwtService = TestingEnvironment.ServiceProvider.GetRequiredService<IJwtService>();
	}


	[Fact]
	public async Task GenerateToken_With_ExistedUser()
	{
		var user = MockUsers.Default;
		var result = await _jwtService.GenerateAsync(user);

		AssertAuthResult.ValidResult(result, user);
	}
}