namespace CleanTemplate.Application.Models;

public record struct JwtToken(string Token, DateTime Expires);