namespace PhotoSiTest.API.HostedServices;

public class DataSeedingOptions
{
    public const string ConfigSectionName = "DataSeeding";

    public int AddressesPerUser { get; set; } = 2;

    public int CategoriesCount { get; set; } = 3;

    public int MaxOrdersPerAddress { get; set; } = 3;

    public decimal MaxProductPrice { get; set; } = 1000.00m;

    public int MaxProductsPerOrder { get; set; } = 5;

    public decimal MinProductPrice { get; set; } = 10.00m;

    public int ProductsPerCategory { get; set; } = 3;

    public int UsersCount { get; set; } = 3;
}
