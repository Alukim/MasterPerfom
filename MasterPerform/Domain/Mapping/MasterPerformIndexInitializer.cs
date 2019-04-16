using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MasterPerform.Domain.Entities;
using MasterPerform.Infrastructure.ElasticSearch;
using Microsoft.Extensions.Options;
using Nest;

namespace MasterPerform.Domain.Mapping
{
    public class MasterPerformIndexInitializer : IIndexInitializer
    {
        private readonly IElasticClient elasticClient;
        private readonly IIndexNameProvider<User> indexNameProvider;
        private readonly IOptions<ElasticSearchSettings> elasticSearchSettings;

        public void InitializeIndex()
        {
            if (!elasticClient.IndexExists(indexNameProvider.IndexName).Exists)
            {
                var elasticResponse = elasticClient.CreateIndex(indexNameProvider.IndexName, x => x
                    .Settings(s => s
                        .Setting("index.mapper.dynamic", true)
                        //.DefaultSettings()
                        .NumberOfShards(elasticSearchSettings.Value.ShardsNumber))
                    .Mappings(m => m.MapUsers()));
                if (!elasticResponse.IsValid)
                    throw new Exception($"Error on creating index {indexNameProvider.IndexName}.\n Error: {elasticResponse.DebugInformation}");
            }
        }
    }

    public static class MappingDescriptionExtensions
    {
        internal static MappingsDescriptor MapUsers(this MappingsDescriptor mappingsDescriptor)
            => mappingsDescriptor
                .Map<User>(x => x
                    .AutoMap()
                    .MapKeyword(z => z.Id));


        public static TypeMappingDescriptor<TObject> MapKeyword<TObject>(
            this TypeMappingDescriptor<TObject> descriptor, Expression<Func<TObject, object>> field)
            where TObject : class
        {
            return descriptor.Properties(p => p.Keyword(s => s.Name(field)));
        }
    }
}
