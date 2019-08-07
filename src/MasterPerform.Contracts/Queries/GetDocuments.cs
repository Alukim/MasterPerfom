using MasterPerform.Contracts.Responses;
using MasterPerform.Infrastructure.Messaging.Contracts;
using System.Collections.Generic;

namespace MasterPerform.Contracts.Queries
{
    /// <summary>
    /// Query used to get list of documents.
    /// </summary>
    public class GetDocuments : IQuery<IReadOnlyCollection<DocumentResponse>>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="query">Query phrase.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="pageNumber">Page number.</param>
        public GetDocuments(string query, int pageSize, int pageNumber)
        {
            Query = query;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }

        /// <summary>
        /// Query phrase.
        /// </summary>
        public string Query { get; }

        /// <summary>
        /// Page size.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Page number.
        /// </summary>
        public int PageNumber { get; }
    }
}
