using MasterPerform.Infrastructure.Entities;
using MasterPerform.Infrastructure.EnvironmentPrefixer;
using Nest;

namespace MasterPerform.Infrastructure.Elasticsearch
{
    public interface IIndexNameResolver
    {
        string GetIndexNameFor<TIndex>() where TIndex : class, IEntity;
    }

    internal class EnvironmentIndexNameResolver : IIndexNameResolver
    {
        private readonly IndexNameResolver resolver;
        private readonly IEnvironmentPrefixer prefixer;

        public EnvironmentIndexNameResolver(IEnvironmentPrefixer prefixer, IndexNameResolver resolver)
        {
            this.prefixer = prefixer;
            this.resolver = resolver;
        }


        public string GetIndexNameFor<TIndex>()
            where TIndex : class, IEntity
        {
            var indexName = resolver.Resolve<TIndex>();
            return prefixer.AppendPrefix(indexName);
        }
    }
}
