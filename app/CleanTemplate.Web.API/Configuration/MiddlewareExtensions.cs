using CleanTemplate.Core.Tools;
using CleanTemplate.Web.Api.Middleware;

namespace CleanTemplate.Web.Api.Configuration;

public static class MiddlewareExtensions
{
	public static void AddFactoryMiddleware(this IServiceCollection services)
	{
		IEnumerable<Type> middlewares = AssemblyProvider.GetImplementations<IMiddleware>();
		foreach (Type middleware in middlewares)
			services.AddScoped(middleware);
	}

	public static void UseCustomExceptionHandler(this IApplicationBuilder app)
	{
		app.UseMiddleware<ExceptionHandlerMiddleware>();
	}
}