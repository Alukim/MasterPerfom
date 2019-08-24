using Nest;

namespace MasterPerform.Infrastructure.Elasticsearch.Descriptors.Definitions
{
    public interface ISimilarField
    {
        Field ContainsField { get; }
        Field StartsWithField { get; }
        Field ExactMatchField { get; }
    }
}
