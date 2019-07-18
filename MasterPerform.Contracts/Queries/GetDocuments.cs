using MasterPerform.Contracts.Responses;
using MasterPerform.Infrastructure.Messaging.Contracts;
using System.Collections.Generic;

namespace MasterPerform.Contracts.Queries
{
    public class GetDocuments : IQuery<IReadOnlyCollection<DocumentResponse>>
    {
        public GetDocuments(string query, int pageSize, int pageNumber)
        {
            Query = query;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }

        public string Query { get; }
        public int PageSize { get; }
        public int PageNumber { get; }
    }
}
