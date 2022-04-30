using CleanTemplate.Core.Abstractions.Context;
using CleanTemplate.Core.Extensions;
using CleanTemplate.Data.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.Data;

public static class DependencyInjection
{
	public static void AddSqlServerDatabase(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext(configuration);
	}


	private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		if (configuration.IsTesting()) return;

		var contextFactory = new SqlServerDbContextFactory();
		services.AddDbContext<SqlServerDbContext>(cob => contextFactory.ConfigureContextOptions(cob));

		services.AddScoped<IDatabaseContext, SqlServerDbContext>();
	}
}