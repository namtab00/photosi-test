using Microsoft.EntityFrameworkCore;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Products.Domain;

namespace PhotoSiTest.Products.Persistence;

public class ProductCategoryRepository(ProductsDbContext context)
    : RepositoryBase<ProductCategory, ProductsDbContext>(context), IProductCategoryRepository
{
    public override async Task<ProductCategory?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await Context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id, cancellationToken: ct);
    }
}
