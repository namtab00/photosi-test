using System.ComponentModel.DataAnnotations;

namespace PhotoSiTest.Contracts.Domain.Users.Dtos;

public record CreateUserDto(
    [Required] [MaxLength(UserConstants.EmailMaxLength)]
    string Email,
    [Required] [MaxLength(UserConstants.FirstNameMaxLength)]
    string FirstName,
    [Required] [MaxLength(UserConstants.LastNameMaxLength)]
    string LastName);
