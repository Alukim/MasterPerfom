using MasterPerform.Infrastructure.ElasticSearch.Mappings;
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

        public static AnalyzersDescriptor NGram(this AnalyzersDescriptor descriptor)
            => descriptor.Custom(CustomAnalyzers.NGRAM_ANALYZER, zz => zz
                .Tokenizer(CustomTokenizers.NGRAM)
                .Filters(CustomFilters.ASCIIFOLDING, CustomFilters.LOWERCASE));
    }
}
