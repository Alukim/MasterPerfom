using MasterPerform.Entities;
using MasterPerform.Infrastructure.Elasticsearch.Descriptors;
using System.Linq;

namespace MasterPerform.Descriptors
{
    public class MasterPerformFindSimilarDescriptor : FindSimilarDescriptor<Document>
    {
        public MasterPerformFindSimilarDescriptor() : base()
        {
            RegisterSingleField(z => z.Details.FirstName, z => z.Details?.FirstName);
            RegisterSingleField(z => z.Details.LastName, z => z.Details?.LastName);
            RegisterSingleField(z => z.Details.Email, z => z.Details?.Email);
            RegisterSingleField(z => z.Details.Phone, z => z.Details?.Phone);

            RegisterCollectionField(z => z.Addresses.First().AddressLine, z => z.Addresses?.Select(x => x?.AddressLine));
            RegisterCollectionField(z => z.Addresses.First().City, z => z.Addresses?.Select(x => x?.City));
            RegisterCollectionField(z => z.Addresses.First().State, z => z.Addresses?.Select(x => x?.State));
        }

        public static MasterPerformFindSimilarDescriptor Instance => new MasterPerformFindSimilarDescriptor();
    }
}
