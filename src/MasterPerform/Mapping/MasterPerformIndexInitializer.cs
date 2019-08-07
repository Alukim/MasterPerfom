using MasterPerform.Entities;
using MasterPerform.Infrastructure.Elasticsearch;
using MasterPerform.Infrastructure.Elasticsearch.Mappings.Descriptors;
using MasterPerform.Infrastructure.ElasticSearch;
using MasterPerform.Mapping.Extensions;
using Microsoft.Extensions.Options;
using Nest;
using System;
using MasterPerform.Infrastructure.Elasticsearch.Mappings;

namespace MasterPerform.Mapping
{
    public class MasterPerformIndexInitializer : IIndexInitializer
    {
        private readonly IElasticClient _elasticClient;
        private readonly string _indexName;
        private readonly IOptions<ElasticsearchSettings> _elasticSearchSettings;

        public MasterPerformIndexInitializer(IElasticClient elasticClient, IIndexNameResolver indexNameResolver, IOptions<ElasticsearchSettings> elasticSearchSettings)
        {
            this._elasticClient = elasticClient;
            this._indexName = indexNameResolver.GetIndexNameFor<Document>();
            this._elasticSearchSettings = elasticSearchSettings;
        }

        public void InitializeIndex()
        {
            if (_elasticClient.Indices.Exists(_indexName).Exists)
                return;

            var elasticResponse = _elasticClient.Indices.Create(_indexName, x => x
                .Settings(s => s
                    .Setting(CustomSettings.MAX_NGRAM_DIFF, 100)
                    .NumberOfShards(_elasticSearchSettings.Value.ShardsNumber)
                    .Analysis(a => a
                        .Tokenizers(z => z
                            .NGram())
                        .Analyzers(z => z
                            .StandardLowercase()
                            .KeywordLowercase()
                            .NGram())
                        .Normalizers(n => n
                            .Lowercase())
                    )
                )
                .Map<Document>(m => m.MapDocuments()));
            if (!elasticResponse.IsValid)
                throw new Exception($"Error on creating index {_indexName}.\n Error: {elasticResponse.DebugInformation}");
        }
    }
}
