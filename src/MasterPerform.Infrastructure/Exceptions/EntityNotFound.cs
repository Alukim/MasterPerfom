using System;

namespace MasterPerform.Infrastructure.Exceptions
{
    public class EntityNotFound : MasterPerformException
    {
        public EntityNotFound(string entityName, Guid entityId) :
            base($"Entity {entityName} with id: {entityId} does not exist.")
        {
        }
    }
}
