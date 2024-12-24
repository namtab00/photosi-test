using PhotoSiTest.Common.Data;
using PhotoSiTest.Users.Domain;

namespace PhotoSiTest.Users.Persistence;

public class UserRepository(UsersDbContext context) : RepositoryBase<User, UsersDbContext>(context), IUserRepository;
