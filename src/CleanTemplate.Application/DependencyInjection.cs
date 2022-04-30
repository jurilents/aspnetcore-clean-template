using System.Reflection;
using System.Text;
using CleanTemplate.Application.Options;
using CleanTemplate.Core.Constants;
using CleanTemplate.Core.DependencyInjection;
using CleanTemplate.Data.Context;
using CleanTemplate.Data.Entities;
using FluentValidation.AspNetCore;
using HashidsNet;
using Mapster;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;

namespace CleanTemplate.Application;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddHashids(configuration.GetSection("Hashids").Bind);

		services.RegisterServicesFromAssembly("CleanTemplate.Application");

		services.BindConfigurationOptions(configuration);
		services.RegisterMappings();

		services.AddCustomMediatR();
	}


	public static void ConfigureFluentValidation(FluentValidationMvcConfiguration options)
	{
		options.DisableDataAnnotationsValidation = true;
		options.ImplicitlyValidateChildProperties = true;
		options.ImplicitlyValidateRootCollectionElements = true;
	}

	private static void BindConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<JwtOptions>(options =>
		{
			options.Secret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]));
			options.AccessTokenLifetime = TimeSpan.TryParse(configuration["Jwt:AccessTokenLifetime"], out var val) ? val : TimeSpan.FromDays(10);
			options.RefreshTokenLifetime = TimeSpan.TryParse(configuration["Jwt:RefreshTokenLifetime"], out val) ? val : TimeSpan.FromDays(30);
		});

		services.Configure<LocalizationOptions>(configuration.GetSection("Localization"));
	}

	public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddIdentity<AppUser, AppRole>(configuration.GetRequiredSection("Identity").Bind)
				.AddEntityFrameworkStores<SqlServerDbContext>()
				.AddTokenProvider(TokenProviders.Default, typeof(EmailTokenProvider<AppUser>));

		services.Configure<PasswordHasherOptions>(option => option.IterationCount = 7000);
	}

	private static void RegisterMappings(this IServiceCollection services)
	{
		var serviceProvider = services.BuildServiceProvider();
		var hashids = serviceProvider.GetRequiredService<IHashids>();
		var register = new MappingRegister(hashids);

		register.Register(TypeAdapterConfig.GlobalSettings);
	}

	private static void AddCustomMediatR(this IServiceCollection services)
	{
		var assemblies = new[] { Assembly.GetExecutingAssembly() };

		services.AddMediatR(assemblies).AddFluentValidation(ConfigureFluentValidation);
		services.AddFluentValidation(assemblies);
	}
}