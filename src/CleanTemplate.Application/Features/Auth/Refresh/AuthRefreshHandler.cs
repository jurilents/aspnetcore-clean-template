using CleanTemplate.Application.Services;
using MediatR;

namespace CleanTemplate.Application.Features.Auth;

internal class AuthRefreshHandler : IRequestHandler<AuthRefreshCommand, AuthResult>
{
	private readonly IJwtService _jwtService;
	public AuthRefreshHandler(IJwtService jwtService) => _jwtService = jwtService;


	public async Task<AuthResult> Handle(AuthRefreshCommand request, CancellationToken cancel)
	{
		return await _jwtService.RefreshAsync(request.RefreshToken, cancel);
	}
}