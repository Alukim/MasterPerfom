using Nest;

namespace MasterPerform.Infrastructure.Elasticsearch.Mappings.Descriptors
{
    public static class NormalizersDescriptors
    {
        public static NormalizersDescriptor Lowercase(this NormalizersDescriptor descriptor)
            => descriptor.Custom(CustomNormalizers.KEYWORD_LOWERCASE, zz => zz
                .Filters(CustomFilters.LOWERCASE));
    }
}
