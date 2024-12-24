namespace PhotoSiTest.Common.Data;

public interface IContextMigrator
{
    string ContextTypeName { get; }


    void Migrate();
}
