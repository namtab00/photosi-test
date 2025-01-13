using Microsoft.Extensions.Logging;
using PhotoSiTest.Contracts.Domain.Users;
using PhotoSiTest.Contracts.Domain.Users.Dtos;
using Refit;

namespace PhotoSiTest.Users.Services;

public class UserServiceProxy(IUsersApiClient apiClient, ILogger<UserServiceProxy> logger) : IUserServiceProxy
{
    public async Task<UserDto> CreateUserAsync(CreateUserDto dto, CancellationToken ct = default)
    {
        try
        {
            var response = await apiClient.Create(dto, ct);
            await response.EnsureSuccessStatusCodeAsync();

            return response.Content ?? throw new InvalidOperationException("Expected a user creation result");
        }
        catch (ApiException apiException)
        {
            logger.LogCritical(apiException, "Users API Refit exception for {APIUri}", apiException.Uri);
            throw;
        }
        catch (HttpRequestException hex)
        {
            logger.LogCritical(hex, "Users API HTTP exception");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Users API generic exception");
            throw;
        }
    }


    public async Task DeleteUserAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            await apiClient.Delete(id, ct);
        }
        catch (ApiException apiException)
        {
            logger.LogCritical(apiException, "Users API Refit exception for {APIUri}", apiException.Uri);
            throw;
        }
        catch (HttpRequestException hex)
        {
            logger.LogCritical(hex, "Users API HTTP exception");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Users API generic exception");
            throw;
        }
    }


    public async Task<UserDto?> FindUserAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var result = await apiClient.GetById(id, ct);
            await result.EnsureSuccessStatusCodeAsync();

            return result.Content;
        }
        catch (ApiException apiException)
        {
            logger.LogCritical(apiException, "Users API Refit exception for {APIUri}", apiException.Uri);
            throw;
        }
        catch (HttpRequestException hex)
        {
            logger.LogCritical(hex, "Users API HTTP exception");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Users API generic exception");
            throw;
        }
    }


    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await apiClient.GetAll(ct);
            await result.EnsureSuccessStatusCodeAsync();

            return result.Content ?? throw new InvalidOperationException("Expected a user list result");
        }
        catch (ApiException apiException)
        {
            logger.LogCritical(apiException, "Users API Refit exception for {APIUri}", apiException.Uri);
            throw;
        }
        catch (HttpRequestException hex)
        {
            logger.LogCritical(hex, "Users API HTTP exception");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Users API generic exception");
            throw;
        }
    }


    public async Task<UserDto> GetUserAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var result = await apiClient.GetById(id, ct);
            await result.EnsureSuccessStatusCodeAsync();

            return result.Content ?? throw new InvalidOperationException("Expected a user retrieval result");
        }
        catch (ApiException apiException)
        {
            logger.LogCritical(apiException, "Users API Refit exception for {APIUri}", apiException.Uri);
            throw;
        }
        catch (HttpRequestException hex)
        {
            logger.LogCritical(hex, "Users API HTTP exception");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Users API generic exception");
            throw;
        }
    }


    public async Task<UserDto?> GetUserByEmailAsync(string email, CancellationToken ct = default)
    {
        try
        {
            var result = await apiClient.GetByEmail(email, ct);
            await result.EnsureSuccessStatusCodeAsync();

            return result.Content;
        }
        catch (ApiException apiException)
        {
            logger.LogCritical(apiException, "Users API Refit exception for {APIUri}", apiException.Uri);
            throw;
        }
        catch (HttpRequestException hex)
        {
            logger.LogCritical(hex, "Users API HTTP exception");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Users API generic exception");
            throw;
        }
    }


    public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto dto, CancellationToken ct = default)
    {
        try
        {
            var result = await apiClient.Update(id, dto, ct);
            await result.EnsureSuccessStatusCodeAsync();

            return result.Content ?? throw new InvalidOperationException("Expected a user update result");
        }
        catch (ApiException apiException)
        {
            logger.LogCritical(apiException, "Users API Refit exception for {APIUri}", apiException.Uri);
            throw;
        }
        catch (HttpRequestException hex)
        {
            logger.LogCritical(hex, "Users API HTTP exception");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Users API generic exception");
            throw;
        }
    }
}
