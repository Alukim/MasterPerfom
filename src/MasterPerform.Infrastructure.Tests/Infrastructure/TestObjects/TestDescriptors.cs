using MasterPerform.Infrastructure.Elasticsearch.Descriptors;

namespace MasterPerform.Tests.Infrastructure.TestObjects
{
    internal class TestFullTextSearchDescriptor : FullTextSearchDescriptor<TestEntity>
    {
        public static TestFullTextSearchDescriptor Instance => new TestFullTextSearchDescriptor();
    }

    internal class TestFindSimilarDescriptor : FindSimilarDescriptor<TestEntity>
    {
        public static TestFindSimilarDescriptor Instance => new TestFindSimilarDescriptor();
    }
}
