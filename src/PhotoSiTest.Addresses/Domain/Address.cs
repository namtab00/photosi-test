using System.ComponentModel.DataAnnotations;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Contracts.Domain.Addresses;

namespace PhotoSiTest.Addresses.Domain;

public class Address : PhotoSiTestEntity
{
    [MaxLength(AddressConstants.LocationMaxLength)]
    public string Location { get; set; } = null!;

    public Guid UserId { get; set; }
}
