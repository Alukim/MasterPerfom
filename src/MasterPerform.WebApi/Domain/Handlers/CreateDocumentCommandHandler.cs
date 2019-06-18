using MasterPerform.Infrastructure.Messaging.Handlers;
using MasterPerform.WebApi.Contracts;
using System;
using System.Threading.Tasks;

namespace MasterPerform.WebApi.Domain.Handlers
{
    public class CreateDocumentCommandHandler :
        ICommandHandler<CreateDocument>
    {
        public Task Handler(CreateDocument command)
        {
            throw new NotImplementedException();
        }
    }
}
