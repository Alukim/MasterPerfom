using MasterPerform.Infrastructure.ElasticSearch.Mappings;
using Nest;
using System;
using System.Linq.Expressions;

namespace MasterPerform.Infrastructure.Elasticsearch.Mappings
{
    public static class MappingExtensions
    {
        public static TypeMappingDescriptor<TObject> MapKeywordNonIndexed<TObject>(
            this TypeMappingDescriptor<TObject> descriptor, Expression<Func<TObject, object>> field)
            where TObject : class
        {
            return descriptor.Properties(p => p.Keyword(s => s.Name(field).Index(false)));
        }

        public static TypeMappingDescriptor<TObject> MapKeyword<TObject>(
            this TypeMappingDescriptor<TObject> descriptor, Expression<Func<TObject, object>> field)
            where TObject : class
        {
            return descriptor.Properties(p => p.Keyword(s => s.Name(field).Index(true)));
        }

        public static PropertiesDescriptor<TObject> AddExactMatchField<TObject>(
            this PropertiesDescriptor<TObject> descriptor,
            string normalizer = CustomNormalizers.KEYWORD_LOWERCASE)
            where TObject : class
        {
            return descriptor.Keyword(x => x
                .Name(CustomFields.ExactMatchSuffix)
                .Normalizer(normalizer)
                .Store());
        }

        public static PropertiesDescriptor<TObject> AddStartsWithField<TObject>(
            this PropertiesDescriptor<TObject> descriptor,
            string analyzer = CustomNormalizers.KEYWORD_LOWERCASE)
            where TObject : class
        {
            return descriptor.Text(x => x
                .Name(CustomFields.StartWithSuffix)
                .Analyzer(analyzer)
                .SearchAnalyzer(analyzer)
                .SearchQuoteAnalyzer(analyzer)
                .Fielddata()
                .Store());
        }
    }
}
