using PhotoSiTest.Contracts.Domain.Users;
using PhotoSiTest.Contracts.Domain.Users.Dtos;
using Refit;

namespace PhotoSiTest.Users.Services;

[Headers("Content-Type: application/json; charset=utf-8")]
public interface IUsersApiClient
{
    [Post(UserConstants.ApiRoutes.Create)]
    Task<ApiResponse<UserDto>> Create(CreateUserDto request, CancellationToken ct = default);


    [Get(UserConstants.ApiRoutes.Delete)]
    Task Delete(Guid id, CancellationToken ct = default);


    [Get(UserConstants.ApiRoutes.GetAll)]
    Task<ApiResponse<List<UserDto>>> GetAll(CancellationToken ct = default);


    [Get(UserConstants.ApiRoutes.GetByEmail)]
    Task<ApiResponse<UserDto>> GetByEmail([Query] string email, CancellationToken ct = default);


    [Get(UserConstants.ApiRoutes.GetById)]
    Task<ApiResponse<UserDto>> GetById(Guid id, CancellationToken ct = default);


    [Put(UserConstants.ApiRoutes.Update)]
    Task<ApiResponse<UserDto>> Update(Guid id, UpdateUserDto dto, CancellationToken ct = default);
}
