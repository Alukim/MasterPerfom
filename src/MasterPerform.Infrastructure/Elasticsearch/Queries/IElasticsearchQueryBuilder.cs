using MasterPerform.Infrastructure.Entities;
using Nest;

namespace MasterPerform.Infrastructure.Elasticsearch.Queries
{
    public interface IElasticsearchQueryBuilder<TType, TDescriptorType>
        where TType : class
        where TDescriptorType : class, IEntity
    {
        SearchDescriptor<TDescriptorType> BuildQuery(TType container);
    }
}
