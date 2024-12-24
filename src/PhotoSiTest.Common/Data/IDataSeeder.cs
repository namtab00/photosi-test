namespace PhotoSiTest.Common.Data;

public interface IDataSeeder
{
    Task SeedAsync(CancellationToken ct);
}
