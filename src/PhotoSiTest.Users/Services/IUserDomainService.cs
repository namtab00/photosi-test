using PhotoSiTest.Contracts.Domain.Users.Dtos;

namespace PhotoSiTest.Users.Services;

public interface IUserDomainService
{
    Task<UserDto> CreateUserAsync(CreateUserDto dto, CancellationToken ct = default);


    Task DeleteUserAsync(Guid id, CancellationToken ct = default);


    Task<UserDto?> FindUserAsync(Guid id, CancellationToken ct = default);


    Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken ct = default);


    Task<UserDto> GetUserAsync(Guid id, CancellationToken ct = default);


    Task<UserDto?> GetUserByEmailAsync(string email, CancellationToken ct = default);


    Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto dto, CancellationToken ct = default);
}
