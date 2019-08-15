using MasterPerform.Entities;
using MasterPerform.Infrastructure.Elasticsearch.Descriptors;

namespace MasterPerform.Descriptors
{
    public class MasterPerformFindSimilarDescriptor : FindSimilarDescriptor<Document>
    {
        public MasterPerformFindSimilarDescriptor()
        {

        }

        public static MasterPerformFindSimilarDescriptor Instance => new MasterPerformFindSimilarDescriptor();
    }
}
