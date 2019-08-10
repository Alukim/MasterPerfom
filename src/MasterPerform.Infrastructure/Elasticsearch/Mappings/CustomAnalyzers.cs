namespace MasterPerform.Infrastructure.Elasticsearch.Mappings
{
    public class CustomAnalyzers
    {
        public const string STANDARD_LOWERCASE = "standard_lowercase";
        public const string KEYWORD_LOWERCASE = "keyword_lowercase";
        public const string KEYWORD_LOWERCASE_SEARCH = "keyword_lowercase_search";
        public const string ALPHANUMERIC = "alphanumeric";
        public const string NGRAM_ANALYZER = "ngram_analyzer";
        public const string NGRAM_ALPHANUMERIC_ANALYZER = "ngram_alphanumeric_analyzer";
    }
}
