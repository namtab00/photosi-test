using System.ComponentModel.DataAnnotations;

namespace PhotoSiTest.Contracts.Domain.Addresses.Dtos;

public record CreateAddressDto(
    Guid UserId,
    [Required] [MaxLength(AddressConstants.LocationMaxLength)]
    string Location);
