using System.ComponentModel.DataAnnotations;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Contracts.Domain.Users;

namespace PhotoSiTest.Users.Domain;

public class User : PhotoSiTestEntity
{
    [MaxLength(UserConstants.EmailMaxLength)]
    public string Email { get; set; } = null!;

    [MaxLength(UserConstants.FirstNameMaxLength)]
    public string FirstName { get; set; } = null!;

    [MaxLength(UserConstants.LastNameMaxLength)]
    public string LastName { get; set; } = null!;
}
