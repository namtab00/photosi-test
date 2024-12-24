using AutoMapper;
using PhotoSiTest.Addresses.Domain;
using PhotoSiTest.Addresses.Persistence;
using PhotoSiTest.Common.Exceptions;
using PhotoSiTest.Contracts.Domain.Addresses;
using PhotoSiTest.Contracts.Domain.Addresses.Dtos;
using PhotoSiTest.Contracts.Domain.Users;

namespace PhotoSiTest.Addresses.Services;

public class AddressService(IAddressRepository addressRepository, IUserService userService, IMapper mapper) : IAddressService
{
    public async Task<AddressDto> CreateAddressAsync(CreateAddressDto dto, CancellationToken ct = default)
    {
        _ = await userService.FindUserAsync(dto.UserId, ct) ?? throw new InvalidEntityReferenceException("User", dto.UserId);

        var address = mapper.Map<Address>(dto);

        var result = await addressRepository.AddAsync(address, ct);
        return mapper.Map<AddressDto>(result);
    }


    public async Task DeleteAddressAsync(Guid id, CancellationToken ct = default)
    {
        _ = await addressRepository.GetByIdAsync(id, ct) ?? throw new EntityNotFoundException<Address>(id);

        await addressRepository.DeleteAsync(id, ct);
    }


    public async Task<AddressDto?> FindAddressAsync(Guid id, CancellationToken ct = default)
    {
        var address = await addressRepository.GetByIdAsync(id, ct);
        return address != null ? mapper.Map<AddressDto>(address) : null;
    }


    public async Task<AddressDto> GetAddressAsync(Guid id, CancellationToken ct = default)
    {
        var address = await addressRepository.GetByIdOrThrowAsync(id, ct);
        return mapper.Map<AddressDto>(address);
    }


    public async Task<IEnumerable<AddressDto>> GetAllAddressesAsync(CancellationToken ct = default)
    {
        var addresses = await addressRepository.GetListAsync(ct: ct);
        return mapper.Map<IEnumerable<AddressDto>>(addresses);
    }


    public async Task<IEnumerable<AddressDto>> GetUserAddressesAsync(Guid userId, CancellationToken ct = default)
    {
        var addresses = await addressRepository.GetListAsync(a => a.UserId == userId, ct: ct);
        return mapper.Map<IEnumerable<AddressDto>>(addresses);
    }


    public async Task<AddressDto> UpdateAddressAsync(Guid id, UpdateAddressDto dto, CancellationToken ct = default)
    {
        var address = await addressRepository.GetByIdAsync(id, ct) ?? throw new EntityNotFoundException<Address>(id);

        mapper.Map(dto, address);
        await addressRepository.UpdateAsync(address, ct);
        return mapper.Map<AddressDto>(address);
    }
}
