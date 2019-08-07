using MasterPerform.Contracts.Commands;
using MasterPerform.Contracts.Queries;
using MasterPerform.Contracts.Responses;
using MasterPerform.Infrastructure.Messaging;
using MasterPerform.WebApi.Utilities.Attributes;
using Microsoft.AspNetCore.Mvc;
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
            return Created("/", new {id = command.CreatedId});
        }

        /// <summary>
        /// Get list of documents.
        /// </summary>
        /// <returns>Collection of DocumentResponse.</returns>
        [HttpGet(Name = "DocumentDetails.GetList")]
        [ProducesResponseType(typeof(IReadOnlyCollection<DocumentResponse>), 200)]
        public Task<IReadOnlyCollection<DocumentResponse>> GetList([FromQuery] string query, int pageSize, int pageNumber)
        {
            var getDocuments = new GetDocuments(
                query: query,
                pageSize: pageSize,
                pageNumber: pageNumber);
            return _commandQueryProvider.SendAsync(getDocuments);
        }

        /// <summary>
        /// Get document by id.
        /// </summary>
        /// <param name="query">GetDocument class.</param>
        /// <returns>DocumentResponse</returns>
        [HttpGet("{documentId}", Name = "DocumentDetails.GetDocument")]
        [ProducesResponseType(typeof(DocumentResponse), 200)]
        public Task<DocumentResponse> GetDocument([FromRoute] GetDocument query)
            => _commandQueryProvider.SendAsync(query);

        /// <summary>
        /// Update document details operation.
        /// </summary>
        /// <returns>HTTP 204</returns>
        [HttpPut("{documentId}/details", Name = "DocumentDetails.UpdateDetails")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateDocumentDetails([FromBody] UpdateDocumentDetails command)
        {
            await _commandQueryProvider.SendAsync(command);
            return NoContent();
        }

        /// <summary>
        /// Update addresses operation
        /// </summary>
        /// <returns>HTTP 204</returns>
        [HttpPut("{documentId}/addresses", Name = "DocumentDetails.UpdateAddresses")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateDocumentAddresses([FromBody] UpdateDocumentAddresses command)
        {
            await _commandQueryProvider.SendAsync(command);
            return NoContent();
        }
    }
}
