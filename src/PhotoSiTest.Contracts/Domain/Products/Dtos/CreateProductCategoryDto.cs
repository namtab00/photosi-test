using System.ComponentModel.DataAnnotations;

namespace PhotoSiTest.Contracts.Domain.Products.Dtos;

public record CreateProductCategoryDto(
    [Required] [MaxLength(ProductCategoryConstants.NameMaxLength)]
    string Name,
    [Required] [MaxLength(ProductCategoryConstants.DescriptionMaxLength)]
    string Description);
