using PhotoSiTest.Common.Data;

namespace PhotoSiTest.Common.Exceptions;

public class InvalidEntityReferenceException(string entityTypeName, Guid entityId) : EntityException(entityTypeName, entityId)
{
    public override string Message {
        get {
            return $"Referenced {EntityTypeName} '{EntityId}' not found";
        }
    }
}


public class InvalidEntityReferenceException<T>(Guid entityId) : InvalidEntityReferenceException(typeof(T).Name, entityId)
    where T : PhotoSiTestEntity;
