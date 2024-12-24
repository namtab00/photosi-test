using System.ComponentModel.DataAnnotations;

namespace PhotoSiTest.Contracts.Domain.Products.Dtos;

public record CreateProductDto(
    [Required] [MaxLength(ProductConstants.NameMaxLength)]
    string Name,
    [Required] [MaxLength(ProductConstants.DescriptionMaxLength)]
    string Description,
    [Required] decimal UnitPrice,
    Guid CategoryId);
