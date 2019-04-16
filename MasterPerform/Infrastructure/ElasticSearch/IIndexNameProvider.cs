using MasterPerform.Infrastructure.Entities;

namespace MasterPerform.Infrastructure.ElasticSearch
{
    public interface IIndexNameProvider<TIndex>
        where TIndex : IEntity
    {
        string IndexName { get; }
    }

    internal class IndexNameProvider<TIndex> : IIndexNameProvider<TIndex>
        where TIndex : IEntity
    {
        public IndexNameProvider(string indexName)
            => IndexName = indexName;

        public string IndexName { get; set; }
    }
}
