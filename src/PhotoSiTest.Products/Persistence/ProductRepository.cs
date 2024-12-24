using Microsoft.EntityFrameworkCore;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Products.Domain;

namespace PhotoSiTest.Products.Persistence;

public class ProductRepository(ProductsDbContext context) : RepositoryBase<Product, ProductsDbContext>(context), IProductRepository
{
    public override async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await Context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id, cancellationToken: ct);
    }
}
