using Microsoft.EntityFrameworkCore;
using PhotoSiTest.Common.Data;
using PhotoSiTest.Orders.Domain;

namespace PhotoSiTest.Orders.Persistence;

public class OrdersRepository(OrdersDbContext context) : RepositoryBase<Order, OrdersDbContext>(context), IOrderRepository
{
    public override async Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await Context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id, cancellationToken: ct);
    }
}
