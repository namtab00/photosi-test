namespace PhotoSiTest.Common.Options;

public class PostgresOptions
{
    public const string ConfigSectionName = "Postgres";

    public string ConnectionString { get; set; } = null!;

    public bool EnableAutoMigration { get; set; }

    public bool EnableDataSeeding { get; set; }
}
