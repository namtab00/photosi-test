using System.ComponentModel.DataAnnotations;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Contracts.Domain.Products;

namespace PhotoSiTest.Products.Domain;

public class ProductCategory : PhotoSiTestEntity
{
    [MaxLength(ProductCategoryConstants.DescriptionMaxLength)]
    public string? Description { get; set; }

    [MaxLength(ProductCategoryConstants.NameMaxLength)]
    public string Name { get; set; } = null!;

    public ICollection<Product> Products { get; set; } = [];
}
