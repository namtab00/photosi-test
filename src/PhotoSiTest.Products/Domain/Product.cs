using System.ComponentModel.DataAnnotations;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Contracts.Domain.Products;

namespace PhotoSiTest.Products.Domain;

public class Product : PhotoSiTestEntity
{
    public ProductCategory Category { get; set; } = null!;

    public Guid CategoryId { get; set; }

    [MaxLength(ProductConstants.DescriptionMaxLength)]
    public string? Description { get; set; }

    [MaxLength(ProductConstants.NameMaxLength)]
    public string Name { get; set; } = null!;

    public decimal UnitPrice { get; set; }
}
