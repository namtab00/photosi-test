using System.ComponentModel.DataAnnotations;

namespace PhotoSiTest.Contracts.Domain.Addresses.Dtos;

public record UpdateAddressDto(
    [Required] [MaxLength(AddressConstants.LocationMaxLength)]
    string Location);
