using MasterPerform.Entities;
using MasterPerform.Infrastructure.Elasticsearch.Mappings;
using Nest;
using System;
using System.Linq.Expressions;

namespace MasterPerform.Mapping.Extensions
{
    public static class DocumentMappingExtensions
    {
        internal static TypeMappingDescriptor<Document> MapDocuments(
    this TypeMappingDescriptor<Document> mappingsDescriptor)
    => mappingsDescriptor
        .AutoMap()
        .MapKeywordNonIndexed(z => z.Id)
        .Properties(p => p
            .Object<DocumentDetails>(pp => pp
                .Name(z => z.Details)
                .Properties(ppp => ppp
                    .MapTextWithMatchingFields(z => z.FirstName)
                    .MapTextWithMatchingFields(z => z.LastName)
                    .MapTextWithMatchingFields(z => z.Email)
                    .MapTextWithMatchingFields(z => z.Phone))))
        .Properties(p => p
            .Nested<Address>(pp => pp
                .Name(z => z.Addresses)
                .Properties(ppp => ppp
                    .MapTextWithMatchingFields(zz => zz.City)
                    .MapTextWithMatchingFields(zz => zz.AddressLine)
                    .MapTextWithMatchingFields(zz => zz.State)
                ))
        );

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
            string textAnalyzer = CustomAnalyzers.STANDARD_LOWERCASE,
            string searchAnalyzer = CustomAnalyzers.STANDARD_LOWERCASE,
            string exactMatchNormalizer = CustomNormalizers.KEYWORD_LOWERCASE,
            string startWithAnalyzer = CustomAnalyzers.KEYWORD_LOWERCASE)
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
    }
}
