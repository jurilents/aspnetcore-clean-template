using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using static CleanTemplate.Tests.Application.TestingEnvironmentFactory;

namespace CleanTemplate.Tests.Application;

public static class TestingEnvironment
{
	private static IConfiguration? cachedConfiguration;
	private static IWebHostEnvironment? cachedEnvironment;
	private static IServiceProvider? cachedServiceProvider;

	static TestingEnvironment()
	{
		Directory.SetCurrentDirectory("../../../../../app/CleanTemplate.Api");
	}

	public static IServiceProvider ServiceProvider => cachedServiceProvider ??= BuildServiceProvider(Configuration, Environment);
	public static IConfiguration Configuration => cachedConfiguration ??= BuildConfiguration();
	public static IWebHostEnvironment Environment => cachedEnvironment ??= BuildEnvironment();
}