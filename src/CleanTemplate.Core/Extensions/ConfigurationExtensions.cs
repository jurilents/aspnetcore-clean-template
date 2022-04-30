using Microsoft.Extensions.Configuration;

namespace CleanTemplate.Core.Extensions;

public static class ConfigurationExtensions
{
	public static bool IsTesting(this IConfiguration configuration)
	{
		return configuration["TestingMode"] == "true";
	}
}