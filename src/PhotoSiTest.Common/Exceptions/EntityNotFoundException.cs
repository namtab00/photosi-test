using PhotoSiTest.Common.Data;

namespace PhotoSiTest.Common.Exceptions;

public class EntityNotFoundException<T>(Guid entityId) : EntityNotFoundException(typeof(T).Name, entityId)
    where T : PhotoSiTestEntity;


public class EntityNotFoundException(string entityTypeName, Guid entityId) : EntityException(entityTypeName, entityId)
{
    public override string Message {
        get {
            return $"{EntityTypeName} '{EntityId}' not found";
        }
    }
}
