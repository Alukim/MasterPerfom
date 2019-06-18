using System;
using System.Linq.Expressions;
using MasterPerform.Infrastructure.ElasticSearch;
using MasterPerform.WebApi.Domain.Entities;
using Microsoft.Extensions.Options;
using Nest;

namespace MasterPerform.WebApi.Domain.Mapping
{
    public class MasterPerformIndexInitializer : IIndexInitializer
    {
        private readonly IElasticClient elasticClient;
        private readonly string indexName;
        private readonly IOptions<ElasticSearchSettings> elasticSearchSettings;

        public MasterPerformIndexInitializer(IElasticClient elasticClient, IndexNameResolver indexNameResolver, IOptions<ElasticSearchSettings> elasticSearchSettings)
        {
            this.elasticClient = elasticClient;
            this.indexName = indexNameResolver.Resolve<Document>();
            this.elasticSearchSettings = elasticSearchSettings;
        }

        public void InitializeIndex()
        {
            if (!elasticClient.IndexExists(indexName).Exists)
            {
                var elasticResponse = elasticClient.CreateIndex(indexName, x => x
                    .Settings(s => s
                        .Setting("index.mapper.dynamic", false)
                        .NumberOfShards(elasticSearchSettings.Value.ShardsNumber)
                        .Analysis(a => a
                            .Analyzers(
                                z => z
                                    .Custom(
                                        CustomElasticsearchAnalizers.STANDARD_LOWERCASE,
                                        zz => zz
                                            .Tokenizer(ElasticsearchTokenizers.STANDARD)
                                            .Filters(ElasticsearchFilters.LOWERCASE))
                                    .Custom(
                                        CustomElasticsearchAnalizers.KEYWORD_LOWERCASE,
                                        zz => zz
                                            .Tokenizer(ElasticsearchTokenizers.KEYWORD)
                                            .Filters(ElasticsearchFilters.LOWERCASE)
                                        )
                                )
                            .Normalizers(n => n
                                .Custom(
                                    CustomElasticsearchNormalizers.KEYWORD_LOWERCASE,
                                    zz => zz
                                        .Filters(ElasticsearchFilters.LOWERCASE)))
                        )
                    )
                    .Mappings(m => m.MapDocuments()));
                if (!elasticResponse.IsValid)
                    throw new Exception($"Error on creating index {indexName}.\n Error: {elasticResponse.DebugInformation}");
            }
        }
    }

    public class ElasticsearchMappingSettings
    {
        public const string ExactMatchSuffix = "exactMatch";
        public const string StartWithSuffix = "startsWith";
    }

    public class ElasticsearchTokenizers
    {
        public const string STANDARD = "standard";
        public const string KEYWORD = "keyword";
    }

    public class ElasticsearchFilters
    {
        public const string LOWERCASE = "lowercase";
    }

    public class CustomElasticsearchAnalizers
    {
        public const string STANDARD_LOWERCASE = "standard_lowercase";
        public const string KEYWORD_LOWERCASE = "keyword_lowercase";
    }

    public class CustomElasticsearchNormalizers
    {
        public const string KEYWORD_LOWERCASE = "keyword_lowercase";
    }

    public static class MappingDescriptionExtensions
    {
        internal static MappingsDescriptor MapDocuments(this MappingsDescriptor mappingsDescriptor)
            => mappingsDescriptor
                .Map<Document>(x => x
                    .AutoMap()
                    .MapKeywordNonIndexed(z => z.Id)
                    .MapTextWithMatchingFields(z => z.FirstName)
                    .MapTextWithMatchingFields(z => z.LastName)
                    .MapTextWithMatchingFields(z => z.Email)
                    .MapTextWithMatchingFields(z => z.Phone)
                    .Properties(p => p
                        .Nested<Address>(pp => pp
                            .Name(z => z.Addresses)
                            .Properties(ppp => ppp
                                .MapTextWithMatchingFields(zz => zz.City)
                                .MapTextWithMatchingFields(zz => zz.AddressLine)
                                .MapTextWithMatchingFields(zz => zz.State)
                            )))
                );


        public static TypeMappingDescriptor<TObject> MapKeywordNonIndexed<TObject>(
            this TypeMappingDescriptor<TObject> descriptor, Expression<Func<TObject, object>> field)
            where TObject : class
        {
            return descriptor.Properties(p => p.Keyword(s => s.Name(field).Index(false)));
        }

        public static TypeMappingDescriptor<TObject> MapTextWithMatchingFields<TObject>(
            this TypeMappingDescriptor<TObject> descriptor,
            Expression<Func<TObject, object>> field)
            where TObject : class
        {
            return descriptor.Properties(p => p.MapTextWithMatchingFields(field));
        }

        public static PropertiesDescriptor<TObject> MapTextWithMatchingFields<TObject>(
            this PropertiesDescriptor<TObject> descriptor,
            Expression<Func<TObject, object>> field,
            string textAnalyzer = CustomElasticsearchAnalizers.STANDARD_LOWERCASE,
            string searchAnalyzer = CustomElasticsearchAnalizers.STANDARD_LOWERCASE,
            string exactMatchNormalizer = CustomElasticsearchNormalizers.KEYWORD_LOWERCASE,
            string startWithAnalyzer = CustomElasticsearchAnalizers.KEYWORD_LOWERCASE)
            where TObject : class
        {
            return descriptor.Text(s => s
                .Name(field)
                .Analyzer(textAnalyzer)
                .SearchAnalyzer(searchAnalyzer)
                .SearchQuoteAnalyzer(searchAnalyzer)
                .Fields(x => x
                    .AddExactMatchField(exactMatchNormalizer)
                    .AddStartsWithField(startWithAnalyzer)
                )
                .Fielddata()
                .Store()
            );
        }

        public static PropertiesDescriptor<TObject> AddExactMatchField<TObject>(
            this PropertiesDescriptor<TObject> descriptor,
            string normalizer = CustomElasticsearchNormalizers.KEYWORD_LOWERCASE)
            where TObject : class
        {
            return descriptor.Keyword(x => x
                    .Name(ElasticsearchMappingSettings.ExactMatchSuffix)
                    .Normalizer(normalizer)
                    .Store());
        }

        public static PropertiesDescriptor<TObject> AddStartsWithField<TObject>(
            this PropertiesDescriptor<TObject> descriptor,
            string analyzer = CustomElasticsearchAnalizers.KEYWORD_LOWERCASE)
            where TObject : class
        {
            return descriptor.Text(x => x
                .Name(ElasticsearchMappingSettings.StartWithSuffix)
                .Analyzer(analyzer)
                .SearchAnalyzer(analyzer)
                .SearchQuoteAnalyzer(analyzer)
                .Fielddata()
                .Store());
        }
    }
}
