﻿using System.Reflection;
using System.Runtime.Serialization;

namespace CleanTemplate.Core.Extensions;

public static class EnumExtensions
{
	/// <summary>
	/// Converts enum to string by <see cref="EnumMemberAttribute"/> attribute value.
	///	<br/><br/>
	/// Use this method with care, because if the attribute is not specified, an error will be thrown.
	/// If you are sure that everything is OK, then you can use it, and if not, then it is better to
	/// look at the safe <see cref="SafeGetDisplayName{TEnum}">SafeGetDisplayName</see> method.
	/// </summary>
	/// <param name="value">Enum member (for example <see cref="DayOfWeek"/>.<see cref="DayOfWeek.Sunday"/>)</param>
	/// <returns>String value for given enum member</returns>
	public static string GetDisplayName<TEnum>(this TEnum value)
			where TEnum : Enum
	{
		return typeof(TEnum)
				.GetMember(value.ToString()).Single()
				.GetCustomAttribute<EnumMemberAttribute>()!.Value!;
	}

	/// <summary>
	/// Converts enum to string by <see cref="EnumMemberAttribute"/> attribute value
	/// or returns default ToString method invocation result.
	/// </summary>
	/// <param name="value">Enum member (for example <see cref="DayOfWeek"/>.<see cref="DayOfWeek.Sunday"/>)</param>
	/// <returns>String value for given enum member</returns>
	public static string SafeGetDisplayName<TEnum>(this TEnum value)
			where TEnum : Enum
	{
		return value.GetType()
				.GetMember(value.ToString()).Single()
				.GetCustomAttribute<EnumMemberAttribute>()?.Value ?? value.ToString();
	}
}