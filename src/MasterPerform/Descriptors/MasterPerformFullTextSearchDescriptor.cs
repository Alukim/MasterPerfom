using MasterPerform.Entities;
using MasterPerform.Infrastructure.Elasticsearch.Descriptors;
using System.Linq;

namespace MasterPerform.Descriptors
{
    public class MasterPerformFullTextSearchDescriptor : FullTextSearchDescriptor<Document>
    {
        public MasterPerformFullTextSearchDescriptor()
        {
            RegisterFullTextSearchDefinition(z => z.Details.FirstName);
            RegisterFullTextSearchDefinition(z => z.Details.LastName);
            RegisterFullTextSearchDefinition(z => z.Details.Email);
            RegisterFullTextSearchDefinition(z => z.Details.Phone);

            RegisterNestedFullTextSearchDefinition(z => z.Addresses.First().AddressLine, z => z.Addresses);
            RegisterNestedFullTextSearchDefinition(z => z.Addresses.First().City, z => z.Addresses);
            RegisterNestedFullTextSearchDefinition(z => z.Addresses.First().State, z => z.Addresses);
        }

        public static MasterPerformFullTextSearchDescriptor Instance => new MasterPerformFullTextSearchDescriptor();
    }
}
