using CleanTemplate.Application.Abstractions;
using CleanTemplate.Application.Extensions;
using CleanTemplate.Core.Constants;
using CleanTemplate.Core.Exceptions;
using CleanTemplate.Data.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace CleanTemplate.Application.Features.Users;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, User>
{
	private readonly UserManager<AppUser> _userManager;
	public CreateUserHandler(UserManager<AppUser> userManager) => _userManager = userManager;


	public async Task<User> Handle(CreateUserCommand command, CancellationToken cancel)
	{
		var user = new AppUser { UserName = command.Username, Email = command.Email };

		IdentityResult? result = await _userManager.CreateAsync(user);
		if (!result.Succeeded)
			throw new ValidationFailedException("User not created.", result.ToErrorDetails());

		await _userManager.AddPasswordAsync(user, command.Password);
		await _userManager.AddToRoleAsync(user, Roles.User);

		return user.Adapt<User>();
	}
}