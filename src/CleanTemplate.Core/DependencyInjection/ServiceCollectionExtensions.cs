﻿using CleanTemplate.Core.Extensions;
using CleanTemplate.Core.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static void RegisterServicesFromAssembly(this IServiceCollection services, string assemblyName)
	{
		IEnumerable<Type> serviceTypes = AssemblyProvider.GetTypes(assemblyName);

		foreach (Type implType in serviceTypes)
		{
			var attr = implType.GetAttribute<InjectAttribute>();
			if (attr is null)
				continue;

			switch (attr.InjectionType)
			{
				case InjectionType.Auto:
				{
					if (implType.GetInterfaces().Length > 0)
						goto case InjectionType.Interface;
					if (implType.BaseType is not { })
						goto case InjectionType.BaseClass;
					goto case InjectionType.Self;
				}
				case InjectionType.Interface:
				{
					attr.ServiceType ??= implType.GetInterfaces().First();
					services.Add(new ServiceDescriptor(attr.ServiceType, implType, attr.Lifetime));
					break;
				}
				case InjectionType.Self:
				{
					services.Add(new ServiceDescriptor(implType, implType, attr.Lifetime));
					break;
				}
				case InjectionType.BaseClass:
				{
					services.Add(new ServiceDescriptor(implType.BaseType!, implType, attr.Lifetime));
					break;
				}
				default:
					throw new ArgumentOutOfRangeException(nameof(attr.InjectionType), "Invalid injection type");
			}
		}
	}
}