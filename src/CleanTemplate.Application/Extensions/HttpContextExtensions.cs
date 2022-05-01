using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace CleanTemplate.Application.Extensions;

public static class HttpContextExtensions
{
	/// <summary>
	/// Gets IP address from request headers
	/// </summary>
	public static IPAddress GetIPAddress(this HttpContext httpContext)
	{
		var headerValue = httpContext.Request.Headers["X-Forwarded-For"];

		if (headerValue == StringValues.Empty)
		{
			// this will always have a value (running locally in development won't have the header)
			return httpContext.Request.HttpContext.Connection.RemoteIpAddress!;
		}

		// when running behind a load balancer you can expect this header
		var header = headerValue.ToString();
		
		// in case the IP contains a port, remove ':' and everything after
		int sepIndex = header.IndexOf(':');
		header = sepIndex == -1 ? header : header.Remove(sepIndex);
		return IPAddress.Parse(header);
	}

	/// <summary>
	/// Gets user agent from request headers
	/// </summary>
	public static string GetUserAgent(this HttpContext httpContext)
	{
		return httpContext.Request.Headers["User-Agent"].ToString();
	}
}