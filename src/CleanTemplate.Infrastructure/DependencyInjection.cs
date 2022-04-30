using CleanTemplate.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.Infrastructure;

public static class DependencyInjection
{
	public static void AddInfrastructure(this IServiceCollection services)
	{
		services.RegisterServicesFromAssembly("CleanTemplate.Infrastructure");
	}
}