﻿using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CleanTemplate.Core.Abstractions.Context;

/// <summary>
///	Model used to map connection string setting.
/// </summary>
public record AppSettings(IReadOnlyDictionary<string, string> ConnectionStrings);

/// <summary>
///	A factory for creating derived <see cref="SqliteDbContext" /> instances.
/// </summary>
/// <remarks>
/// See <see href="https://aka.ms/efcore-docs-providers">Implementation of database providers and extensions</see>
/// for more information.
/// </remarks>
public abstract class DbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext>
		where TContext : DbContext
{
	/// <summary>
	///	Connection string name in settings file.
	/// </summary>
	public virtual string SelectedConnectionName => "DefaultConnection";

	/// <summary>
	/// Relative path to the json file with connection string configuration.
	/// </summary>
	public abstract string SettingsPath { get; }

	/// <summary>
	///	Connection string used for <see cref="TContext" />.
	/// </summary>
	public virtual string ConnectionString => GetConnectionStringsFromJson(SettingsPath)[SelectedConnectionName];


	/// <summary>
	///	Path to the assembly with migrations.
	/// </summary>
	public virtual string MigrationsAssembly => this.GetType().Assembly.GetName().Name!;


	/// <summary>
	///	Creates a new instance of a derived context.
	/// </summary>
	/// <param name="args">Arguments provided by the design-time service.</param>
	/// <returns>An instance of db context.</returns>
	public abstract TContext CreateDbContext(string[] args);

	/// <summary>
	/// Configures DbContext options
	/// </summary>
	public abstract void ConfigureContextOptions(DbContextOptionsBuilder optionsBuilder);

	/// <summary>
	/// Creates DbContext options
	/// </summary>
	public DbContextOptions CreateContextOptions()
	{
		Console.WriteLine("Database connection used: " + SelectedConnectionName);

		var optionsBuilder = new DbContextOptionsBuilder<TContext>();
		ConfigureContextOptions(optionsBuilder);
		return optionsBuilder.Options;
	}


	/// <summary>
	///	Gets a dictionary of connection strings.
	/// </summary>
	/// <param name="appsettingsPath">Relative path to the json file with connection string configuration.</param>
	/// <returns>Dictionary of all available connection strings.</returns>
	/// <exception cref="FileNotFoundException"></exception>
	protected virtual IReadOnlyDictionary<string, string> GetConnectionStringsFromJson(string appsettingsPath = "appsettings.json")
	{
		string absPath = Path.Combine(Directory.GetCurrentDirectory(), appsettingsPath);

		if (!File.Exists(absPath))
			throw new FileNotFoundException($"Config file not found: '{appsettingsPath}'");

		string json = File.ReadAllText(absPath);
		var settings = JsonSerializer.Deserialize<AppSettings>(json, JsonConventions.ExtendedScheme);
		if (settings is null || settings.ConnectionStrings.Count == 0)
			throw new KeyNotFoundException($"Settings file '{appsettingsPath}' successfully found, but no connection string found here.");

		return settings.ConnectionStrings;
	}
}