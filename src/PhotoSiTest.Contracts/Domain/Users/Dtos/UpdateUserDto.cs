using System.ComponentModel.DataAnnotations;

namespace PhotoSiTest.Contracts.Domain.Users.Dtos;

public record UpdateUserDto(
    [Required] [MaxLength(UserConstants.FirstNameMaxLength)]
    string FirstName,
    [Required] [MaxLength(UserConstants.LastNameMaxLength)]
    string LastName);
