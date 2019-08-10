using Nest;

namespace MasterPerform.Infrastructure.Elasticsearch.Mappings.Descriptors
{
    public static class AnalyzersDescriptors
    {
        public static AnalyzersDescriptor StandardLowercase(this AnalyzersDescriptor descriptor)
            => descriptor.Custom(CustomAnalyzers.STANDARD_LOWERCASE, zz => zz
                .Tokenizer(CustomTokenizers.STANDARD)
                .Filters(CustomFilters.LOWERCASE));

        public static AnalyzersDescriptor KeywordLowercase(this AnalyzersDescriptor descriptor)
            => descriptor.Custom(CustomAnalyzers.KEYWORD_LOWERCASE, zz => zz
                .Tokenizer(CustomTokenizers.KEYWORD)
                .Filters(CustomFilters.LOWERCASE));

        public static AnalyzersDescriptor KeywordLowercaseSearch(this AnalyzersDescriptor descriptor)
            => descriptor.Custom(CustomAnalyzers.KEYWORD_LOWERCASE_SEARCH, zz => zz
                .Tokenizer(CustomTokenizers.KEYWORD)
                .Filters(CustomFilters.LOWERCASE, CustomFilters.ASCIIFOLDING));

        public static AnalyzersDescriptor Alphanumeric(this AnalyzersDescriptor descriptor)
            => descriptor.Custom(CustomAnalyzers.ALPHANUMERIC, ca => ca
                .Tokenizer(CustomTokenizers.KEYWORD)
                .Filters(CustomFilters.ALPHANUMERIC_TOKEN_FILTERS, CustomFilters.LOWERCASE));

        public static AnalyzersDescriptor NGram(this AnalyzersDescriptor descriptor)
            => descriptor.Custom(CustomAnalyzers.NGRAM_ANALYZER, zz => zz
                .Tokenizer(CustomTokenizers.NGRAM)
                .Filters(CustomFilters.ASCIIFOLDING, CustomFilters.LOWERCASE));

        public static AnalyzersDescriptor NGramAlphanumeric(this AnalyzersDescriptor descriptor)
            => descriptor.Custom(CustomAnalyzers.NGRAM_ALPHANUMERIC_ANALYZER, z => z
                .Tokenizer(CustomTokenizers.NGRAM)
                .CharFilters(CustomFilters.ALPHANUMERIC_CHAR_FILTERS)
                .Filters(CustomFilters.LOWERCASE));
    }
}
