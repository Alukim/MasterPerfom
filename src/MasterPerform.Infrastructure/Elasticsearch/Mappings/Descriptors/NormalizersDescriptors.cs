using Nest;

namespace MasterPerform.Infrastructure.Elasticsearch.Mappings.Descriptors
{
    public static class NormalizersDescriptors
    {
        public static NormalizersDescriptor Lowercase(this NormalizersDescriptor descriptor)
            => descriptor.Custom(CustomNormalizers.KEYWORD_LOWERCASE, zz => zz
                .Filters(CustomFilters.LOWERCASE));

        public static NormalizersDescriptor Alphanumeric(this NormalizersDescriptor descriptor)
            => descriptor.Custom(CustomNormalizers.ALPHANUMERIC_NORMALIZER, ca => ca
                .CharFilters(CustomFilters.ALPHANUMERIC_CHAR_FILTERS)
                .Filters(CustomFilters.LOWERCASE));
    }
}
