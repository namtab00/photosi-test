using PhotoSiTest.Contracts.Domain.Addresses.Dtos;

namespace PhotoSiTest.Contracts.Domain.Addresses;

public interface IAddressService
{
    Task<AddressDto> CreateAddressAsync(CreateAddressDto dto, CancellationToken ct = default);


    Task DeleteAddressAsync(Guid id, CancellationToken ct = default);


    Task<AddressDto?> FindAddressAsync(Guid id, CancellationToken ct = default);


    Task<AddressDto> GetAddressAsync(Guid id, CancellationToken ct = default);


    Task<IEnumerable<AddressDto>> GetAllAddressesAsync(CancellationToken ct = default);


    Task<IEnumerable<AddressDto>> GetUserAddressesAsync(Guid userId, CancellationToken ct = default);


    Task<AddressDto> UpdateAddressAsync(Guid id, UpdateAddressDto dto, CancellationToken ct = default);
}
