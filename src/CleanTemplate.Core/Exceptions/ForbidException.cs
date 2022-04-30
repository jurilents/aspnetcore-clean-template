using System.Net;

namespace CleanTemplate.Core.Exceptions;

/// <summary>
/// Status Code: 403
/// </summary>
public class ForbidException : HttpException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;
	public override string ErrorType => "AccessDenied";


	public ForbidException(string message) : base(message) { }
}