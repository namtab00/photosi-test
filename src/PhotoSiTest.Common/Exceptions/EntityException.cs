namespace PhotoSiTest.Common.Exceptions;

public class EntityException(string entityTypeName, Guid entityId) : CustomExceptionBase
{
    public Guid EntityId { get; } = entityId;

    public string EntityTypeName { get; } = entityTypeName;
}