using FluentAssertions;
using MasterPerform.Contracts.Commands;
using MasterPerform.Contracts.Responses;
using MasterPerform.Tests.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MasterPerform.Tests.Documents.API
{
    public static class DocumentApiExtensions
    {
        public static async Task CreateDocument(this HttpClient client, CreateDocument command)
        {
            var response = await client.Post(
                url: "api/master-perform/document",
                content: new
                {
                    command.DocumentDetails,
                    command.Addresses
                });

            command.CreatedId = response.GetCreatedId();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        public static async Task UpdateDocumentDetails(this HttpClient client, UpdateDocumentDetails command)
        {
            var response = await client.Put(
                url: $"api/master-perform/document/{command.DocumentId}/details",
                content: command.Details);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        public static async Task UpdateDocumentAddresses(this HttpClient client, UpdateDocumentAddresses command)
        {
            var response = await client.Put(
                url: $"api/master-perform/document/{command.DocumentId}/addresses",
                content: command.Addresses);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        public static Task<DocumentResponse> GetDocument(this HttpClient client, Guid documentId)
            => client.Get<DocumentResponse>(url: $"api/master-perform/document/{documentId}");

        public static Task<IReadOnlyCollection<DocumentResponse>> GetDocuments(this HttpClient client, string query = null, int? pageSize = null, int? pageNumber = null)
            => client.Get<IReadOnlyCollection<DocumentResponse>>(url: $"api/master-perform/document", queryParams: new {query, pageSize, pageNumber});
    }
}
