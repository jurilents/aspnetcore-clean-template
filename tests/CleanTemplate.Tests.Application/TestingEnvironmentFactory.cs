using System.Net;
using CleanTemplate.Application;
using CleanTemplate.Core.Abstractions.Context;
using CleanTemplate.Data.Context;
using CleanTemplate.Data.Entities;
using CleanTemplate.Data.SeedData;
using CleanTemplate.Infrastructure;
using CleanTemplate.Tests.Application.Mocks;
using CleanTemplate.Tests.Application.SeedData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace CleanTemplate.Tests.Application;

public static class TestingEnvironmentFactory
{
	public static IServiceProvider BuildServiceProvider(IConfiguration configuration, IWebHostEnvironment environment)
	{
		var services = new ServiceCollection();
		services.AddSingleton(configuration);
		services.AddSingleton(environment);
		services.AddTestDatabase();

		services.AddScoped(typeof(ILogger<>), typeof(Logger<>));
		services.AddSingleton<ILoggerFactory, LoggerFactory>();
		services.AddSingleton(_ => new FakeUserManager());
		services.AddSingleton(_ => new FakeSignInManager());

		services.AddApplication(configuration);
		services.AddInfrastructure();

		services.AddSingleton(CreateHttpContext);

		var serviceProvider = services.BuildServiceProvider();

		SeedData(serviceProvider.GetRequiredService<IDatabaseContext>());

		return serviceProvider;
	}

	public static void SeedData(IDatabaseContext context)
	{
		context.Set<AppUser>().AddRange(MockUsers.Data);
		context.Set<AppRole>().AddRange(SeedExtensions.Roles);
		// context.Set<AppRoleClaim>().AddRange(SeedExtensions.RoleClaims);
		context.SaveChanges();
	}

	public static IConfiguration BuildConfiguration()
	{
		return new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
				.AddInMemoryCollection(new Dictionary<string, string>
				{
					["Jwt:Secret"] = "test_abcX1234test1234abcX1234test1234"
				})
				.AddEnvironmentVariables()
				.Build();
	}

	public static IWebHostEnvironment BuildEnvironment(string environmentName = "Testing")
	{
		return Mock.Of<IWebHostEnvironment>(env =>
				env.EnvironmentName == environmentName);
	}

	public static void AddTestDatabase(this IServiceCollection services)
	{
		services.AddDbContext<SqlServerDbContext>(cob =>
					cob.UseInMemoryDatabase(databaseName: "CleanTemplate_TestDB_" + DateTime.Now.ToFileTimeUtc()),
			ServiceLifetime.Transient);
		services.AddTransient<IDatabaseContext, SqlServerDbContext>();
	}

	private static IHttpContextAccessor CreateHttpContext(IServiceProvider provider)
	{
		var mock = new Mock<IHttpContextAccessor>();

		const string url = "https://api.neerspace.com/v1/users?filter=(id==123)&page=1&limit=10";
		UriHelper.FromAbsolute(url, out string scheme, out var host, out var path, out var query, fragment: out _);

		var httpContext = new DefaultHttpContext
		{
			RequestServices = provider,
			Request =
			{
				Scheme = scheme,
				Host = host,
				Path = path,
				QueryString = query,
				Headers =
				{
					["Content-Type"] = "application/json",
					["User-Agent"] = "Xunit/2.4 (Testing Environment 1.0; test)",
					["X-Forwarded-For"] = IPAddress.None.ToString(),
				},
				HttpContext =
				{
					Connection =
					{
						RemoteIpAddress = IPAddress.None,
					},
				}
			}
		};

		mock.SetupGet(o => o.HttpContext).Returns(httpContext);
		return mock.Object;
	}
}