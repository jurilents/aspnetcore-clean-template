using CleanTemplate.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CleanTemplate.Tests.Application.Mocks;

public class FakeUserManager : UserManager<AppUser>
{
	public FakeUserManager()
			: base(new Mock<IUserStore<AppUser>>().Object,
				new Mock<IOptions<IdentityOptions>>().Object,
				new Mock<IPasswordHasher<AppUser>>().Object,
				Array.Empty<IUserValidator<AppUser>>(),
				Array.Empty<IPasswordValidator<AppUser>>(),
				new Mock<ILookupNormalizer>().Object,
				new Mock<IdentityErrorDescriber>().Object,
				new Mock<IServiceProvider>().Object,
				new Mock<ILogger<UserManager<AppUser>>>().Object) { }
}