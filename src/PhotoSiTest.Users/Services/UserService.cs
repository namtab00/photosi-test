using AutoMapper;
using PhotoSiTest.Contracts.Domain.Users;
using PhotoSiTest.Contracts.Domain.Users.Dtos;
using PhotoSiTest.Users.Domain;
using PhotoSiTest.Users.Persistence;

namespace PhotoSiTest.Users.Services;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{
    public async Task<UserDto> CreateUserAsync(CreateUserDto dto, CancellationToken ct = default)
    {
        var conflict = await GetUserByEmailAsync(dto.Email, ct);
        if (conflict != null)
        {
            throw new InvalidOperationException($"User with email {dto.Email} already exists, id '{conflict.Id}'");
        }

        var user = mapper.Map<User>(dto);
        var result = await userRepository.AddAsync(user, ct);
        return mapper.Map<UserDto>(result);
    }


    public async Task DeleteUserAsync(Guid id, CancellationToken ct = default)
    {
        _ = await userRepository.GetByIdOrThrowAsync(id, ct);
        await userRepository.DeleteAsync(id, ct);
    }


    public async Task<UserDto?> FindUserAsync(Guid id, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(id, ct);
        return user != null ? mapper.Map<UserDto>(user) : null;
    }


    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken ct = default)
    {
        var users = await userRepository.GetListAsync(ct: ct);
        return mapper.Map<IEnumerable<UserDto>>(users);
    }


    public async Task<UserDto> GetUserAsync(Guid id, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdOrThrowAsync(id, ct);
        return mapper.Map<UserDto>(user);
    }


    public async Task<UserDto?> GetUserByEmailAsync(string email, CancellationToken ct = default)
    {
        var users = await userRepository.GetListAsync(u => u.Email == email, ct: ct);
        var user = users.FirstOrDefault();
        return user != null ? mapper.Map<UserDto>(user) : null;
    }


    public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto dto, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdOrThrowAsync(id, ct);

        mapper.Map(dto, user);
        await userRepository.UpdateAsync(user, ct);
        return mapper.Map<UserDto>(user);
    }
}
