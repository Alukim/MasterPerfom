using Nest;

namespace MasterPerform.Infrastructure.Elasticsearch.Mappings.Descriptors
{
    public static class FiltersDescriptors
    {
        public static TokenFiltersDescriptor AlphanumericToken(this TokenFiltersDescriptor descriptor)
            => descriptor.PatternReplace(CustomFilters.ALPHANUMERIC_TOKEN_FILTERS, q => q
                .Pattern("([^a-zA-Z0-9])+")
                .Replacement(string.Empty));

        public static CharFiltersDescriptor AlphanumericChar(this CharFiltersDescriptor descriptor)
            => descriptor.PatternReplace(CustomFilters.ALPHANUMERIC_CHAR_FILTERS, q => q
                .Pattern("([^a-zA-Z0-9])+")
                .Replacement(string.Empty));
    }
}
