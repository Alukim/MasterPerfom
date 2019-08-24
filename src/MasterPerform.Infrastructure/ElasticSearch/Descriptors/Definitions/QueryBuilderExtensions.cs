using Nest;
using System;

namespace MasterPerform.Infrastructure.ElasticSearch.Descriptors.Definitions
{
    internal static class QueryBuilderExtensions
    {
        internal static QueryContainer ContainsQuery(Field fieldName, string value, Field nestedField = null)
            => BuildQuery(fieldName, value, 20, QueryType.Contains, nestedField);

        internal static QueryContainer StartsWithQuery(Field fieldName, string value, Field nestedField = null)
            => BuildQuery(fieldName, value, 30, QueryType.StartsWith, nestedField);

        internal static QueryContainer ExactMatchQuery(Field fieldName, string value, Field nestedField = null)
            => BuildQuery(fieldName, value, 50, QueryType.ExactMatch, nestedField);

        internal static QueryContainer BuildQuery(Field fieldName, string value, int boost, QueryType type, Field nestedField)
        {
            QueryContainer internalQuery;

            if (value is null)
                return null;

            switch (type)
            {
                case QueryType.Contains:
                    internalQuery = new MatchQuery
                    {
                        Field = fieldName,
                        Query = value
                    };
                    break;
                case QueryType.StartsWith:
                    internalQuery = new MatchPhrasePrefixQuery
                    {
                        Field = fieldName,
                        Query = value
                    };
                    break;
                case QueryType.ExactMatch:
                    internalQuery = new TermQuery
                    {
                        Field = fieldName,
                        Value = value
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            var transformedQueryContainer = WrapInNestedIfNeeded(internalQuery, nestedField);
            return WrapInConstantQuery(transformedQueryContainer, boost);
        }

        internal static QueryContainer WrapInNestedIfNeeded(QueryContainer query, Field nestedField = null)
        {
            if (nestedField != null)
            {
                return new NestedQuery
                {
                    Path = nestedField,
                    Query = query
                };
            }

            return query;
        }

        internal static QueryContainer WrapInConstantQuery(QueryContainer internalQuery, int boost)
            => new ConstantScoreQuery
            {
                Boost = boost,
                Filter = internalQuery
            };
    }

    internal enum QueryType
    {
        Contains,
        StartsWith,
        ExactMatch
    }
}
