﻿using System.Security.Claims;

namespace CleanTemplate.Core.Constants;

public struct Claims
{
	public const string Id = "sub";
	public const string UserName = "nameid";
	public const string Role = ClaimTypes.Role;
	public const string Permission = "permission";
}