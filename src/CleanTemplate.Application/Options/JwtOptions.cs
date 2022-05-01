using Microsoft.IdentityModel.Tokens;

namespace CleanTemplate.Application.Options;

public class JwtOptions
{
	public SecurityKey? Secret { get; set; }
	public TimeSpan AccessTokenLifetime { get; set; }
	public TimeSpan RefreshTokenLifetime { get; set; }
}