using CleanTemplate.Api.Abstractions;
using CleanTemplate.Application.Features.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanTemplate.Api.Controllers;

[AllowAnonymous]
public class AuthController : ApiController
{
	[HttpPost("check")]
	public async Task<AuthCheckResult> CheckAsync([FromBody] AuthCheckQuery query, CancellationToken cancel) =>
			await Mediator.Send(query, cancel);

	[HttpPost("complete")]
	public async Task<AuthResult> CompleteAsync([FromBody] AuthCompleteCommand command, CancellationToken cancel) =>
			await Mediator.Send(command, cancel);

	[HttpPut("refresh")]
	public async Task<AuthResult> RefreshAsync([FromBody] AuthRefreshCommand command, CancellationToken cancel) =>
			await Mediator.Send(command, cancel);
}