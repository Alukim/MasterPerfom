using MasterPerform.Infrastructure.Elasticsearch.Queries;
using Nest;
using System;

namespace MasterPerform.Tests.Infrastructure.TestObjects
{
    internal class TestQueryBuilder : IElasticsearchQueryBuilder<TestQuery, TestEntity>
    {
        public SearchDescriptor<TestEntity> BuildQuery(TestQuery container)
        {
            throw new NotImplementedException();
        }
    }
}
