using MasterPerform.Infrastructure.Messaging.Contracts;
using MasterPerform.Infrastructure.Messaging.Handlers;
using System;
using System.Threading.Tasks;

namespace MasterPerform.Tests.Infrastructure.TestObjects
{
    public class TestCommand : ICommand {}
    public class TestQuery : IQuery<TestResponse> {}
    public class TestResponse {}

    public class TestHandlers :
        ICommandHandler<TestCommand>,
        IQueryHandler<TestQuery, TestResponse>
    {
        public Task HandleAsync(TestCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<TestResponse> HandleAsync(TestQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
