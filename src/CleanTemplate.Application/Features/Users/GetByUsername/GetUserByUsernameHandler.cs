using CleanTemplate.Core.Abstractions.Context;
using CleanTemplate.Data.Entities;
using CleanTemplate.Data.Extensions;
using Mapster;
using MediatR;

namespace CleanTemplate.Application.Features.Users;

internal class GetUserByUsernameHandler : IRequestHandler<GetUserByUsernameQuery, User>
{
	private readonly IDatabaseContext _database;
	public GetUserByUsernameHandler(IDatabaseContext database) => _database = database;


	public async Task<User> Handle(GetUserByUsernameQuery query, CancellationToken cancel)
	{
		var user = await _database.Set<AppUser>().Where(u => u.UserName == query.Username).FirstOr404Async(cancel);
		return user.Adapt<User>();
	}
}