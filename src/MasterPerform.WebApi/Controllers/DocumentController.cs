using MasterPerform.Contracts.Commands;
using MasterPerform.Contracts.Entities;
using MasterPerform.Contracts.Queries;
using MasterPerform.Contracts.Responses;
using MasterPerform.Infrastructure.Messaging;
using MasterPerform.WebApi.Utilities.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasterPerform.WebApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Controller used to manage document data.
    /// </summary>
    [MasterPerformRoute("document")]
    public class DocumentController : Controller
    {
        private readonly ICommandQueryProvider _commandQueryProvider;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="_commandQueryProvider">Command query provider.</param>
        public DocumentController(ICommandQueryProvider _commandQueryProvider)
            => this._commandQueryProvider = _commandQueryProvider;

        /// <summary>
        /// Create document operation.
        /// </summary>
        /// <param name="command">Create document command.</param>
        /// <returns>HTTP 201 with location header.</returns>
        [HttpPost(Name = "DocumentDetails.Create")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateDocument([FromBody] CreateDocument command)
        {
            await _commandQueryProvider.SendAsync(command);
            return Created($"api/master-perform/document/{command.CreatedId}", new { documentId = command.CreatedId});
        }

        /// <summary>
        /// Get list of documents.
        /// </summary>
        /// <returns>Collection of DocumentResponse.</returns>
        [HttpGet(Name = "DocumentDetails.GetList")]
        [ProducesResponseType(typeof(IReadOnlyCollection<DocumentResponse>), 200)]
        public Task<IReadOnlyCollection<DocumentResponse>> GetList([FromQuery] string query, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            var getDocuments = new GetDocuments(
                query: query,
                pageSize: pageSize,
                pageNumber: pageNumber);
            return _commandQueryProvider.SendAsync<GetDocuments, IReadOnlyCollection<DocumentResponse>>(getDocuments);
        }

        /// <summary>
        /// Get document by id.
        /// </summary>
        /// <param name="documentId">Id of document to get.</param>
        /// <returns>DocumentResponse</returns>
        [HttpGet("{documentId}", Name = "DocumentDetails.GetDocument")]
        [ProducesResponseType(typeof(DocumentResponse), 200)]
        public Task<DocumentResponse> GetDocument([FromRoute] Guid documentId)
            => _commandQueryProvider.SendAsync<GetDocument, DocumentResponse>(new GetDocument(documentId));

        /// <summary>
        /// Update document details operation.
        /// </summary>
        /// <returns>HTTP 204</returns>
        [HttpPut("{documentId}/details", Name = "DocumentDetails.UpdateDetails")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateDocumentDetails([FromRoute] Guid documentId, [FromBody] DocumentDetails documentDetails)
        {
            var command = new UpdateDocumentDetails(documentId, documentDetails);
            await _commandQueryProvider.SendAsync(command);
            return NoContent();
        }

        /// <summary>
        /// Update addresses operation
        /// </summary>
        /// <returns>HTTP 204</returns>
        [HttpPut("{documentId}/addresses", Name = "DocumentDetails.UpdateAddresses")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateDocumentAddresses([FromRoute] Guid documentId, [FromBody] IReadOnlyCollection<Address> addresses)
        {
            var command = new UpdateDocumentAddresses(documentId, addresses);
            await _commandQueryProvider.SendAsync(command);
            return NoContent();
        }
    }
}
