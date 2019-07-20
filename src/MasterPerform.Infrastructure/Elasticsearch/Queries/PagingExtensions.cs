using Nest;

namespace MasterPerform.Infrastructure.Elasticsearch.Queries
{
    public static class PagingExtensions
    {
        public static SearchDescriptor<T> AddPaging<T>(this SearchDescriptor<T> descriptor, int pageSize, int pageNumber)
            where T : class
            => descriptor
                .From((pageNumber - 1) * pageSize)
                .Size(pageSize);
    }
}
