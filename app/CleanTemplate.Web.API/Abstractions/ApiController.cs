using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using ILogger = NLog.ILogger;

namespace CleanTemplate.Web.Api.Abstractions;

/// <summary>
/// Base API controller
/// </summary>
[ApiController]
[ApiVersion(Versions.V1)]
[Route("/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
// [ProducesResponseType(typeof(Error), 400)]
public abstract class ApiController : ControllerBase
{
	private IMediator? _mediator;

	protected ILogger Logger;
	protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

	public ApiController()
	{
		Logger = LogManager.GetLogger(GetType().Name);
	}

	protected void SetNavigationHeaders(int total, int page, int limit)
	{
		Response.Headers["Navigation-Page"] = page.ToString();
		Response.Headers["Navigation-Last-Page"] = ((int) Math.Ceiling((double) total / limit)).ToString();
		Response.Headers["Access-Control-Expose-Headers"] = "Navigation-Page,Navigation-Last-Page";
	}
}