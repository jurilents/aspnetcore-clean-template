using System.Reflection;
using System.Text;
using CleanTemplate.Application.Features.Auth;
using CleanTemplate.Application.Options;
using CleanTemplate.Core.Constants;
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
		services.AddIdentityServices(configuration);
		services.AddHashids(configuration.GetSection("Hashids").Bind);

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
			options.AccessTokenLifetime = TimeSpan.Parse(configuration["Jwt:AccessTokenLifetime"]);
			options.RefreshTokenLifetime = TimeSpan.Parse(configuration["Jwt:RefreshTokenLifetime"]);
		});

		services.Configure<LocalizationOptions>(configuration.GetSection("Localization"));
	}

	private static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddIdentity<AppUser, AppRole>(configuration.GetRequiredSection("Identity").Bind)
				.AddEntityFrameworkStores<SqlServerDbContext>()
				.AddTokenProvider(TokenProviders.Default, typeof(EmailTokenProvider<AppUser>));

		services.Configure<PasswordHasherOptions>(option => option.IterationCount = 10000);
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
		var assemblies = new[]
		{
			AppDomain.CurrentDomain.Load("CleanTemplate.Application"),
			typeof(AuthCompleteCommand).GetTypeInfo().Assembly
		};

		services.AddMediatR(assemblies).AddFluentValidation(ConfigureFluentValidation);
		services.AddFluentValidation(assemblies);
	}
}