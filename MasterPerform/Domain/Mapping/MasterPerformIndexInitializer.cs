using MasterPerform.Domain.Entities;
using MasterPerform.Infrastructure.ElasticSearch;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Linq.Expressions;

namespace MasterPerform.Domain.Mapping
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
                        .Setting("index.mapper.dynamic", true)
                        //.DefaultSettings()
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

    public class ElasticsearchMappingSSettings
    {
        public const string KeywordSuffix = "keyword";
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
                    .MapKeyword(z => z.Id)
                    .MapTextWithKeyword(z => z.FirstName)
                    .MapTextWithKeyword(z => z.LastName)
                    .MapTextWithKeyword(z => z.Email)
                    .MapTextWithKeyword(z => z.Phone)
                    .Properties(p => p
                        .Nested<Address>(pp => pp
                            .Name(z => z.Addresses)
                            .Properties(ppp => ppp
                                .MapTextWithKeyword(zz => zz.City)
                                .MapTextWithKeyword(zz => zz.AddressLine)
                                .MapTextWithKeyword(zz => zz.State)
                            )))
                );


        public static TypeMappingDescriptor<TObject> MapKeyword<TObject>(
            this TypeMappingDescriptor<TObject> descriptor, Expression<Func<TObject, object>> field)
            where TObject : class
        {
            return descriptor.Properties(p => p.Keyword(s => s.Name(field)));
        }

        public static TypeMappingDescriptor<TObject> MapTextWithKeyword<TObject>(
            this TypeMappingDescriptor<TObject> descriptor, Expression<Func<TObject, object>> field)
            where TObject : class
        {
            return descriptor.Properties(p => p.MapTextWithKeyword(field));
        }

        public static PropertiesDescriptor<TObject> MapTextWithKeyword<TObject>(
            this PropertiesDescriptor<TObject> descriptor, Expression<Func<TObject, object>> field)
            where TObject : class
        {
            return descriptor.Text(s => s
                .Name(field)
                .Analyzer(CustomElasticsearchAnalizers.STANDARD_LOWERCASE)
                .SearchAnalyzer(CustomElasticsearchAnalizers.KEYWORD_LOWERCASE)
                .WithKeyword());
        }

        public static TextPropertyDescriptor<TObject> WithKeyword<TObject>(
            this TextPropertyDescriptor<TObject> descriptor)
            where TObject : class
        {
            return descriptor.Fields(x => x.Keyword(xx => xx
                    .Name(ElasticsearchMappingSSettings.KeywordSuffix)
                    .Normalizer(CustomElasticsearchNormalizers.KEYWORD_LOWERCASE)));
        }
    }
}
