using MasterPerform.Contracts.Commands;
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
        {
            this._commandQueryProvider = _commandQueryProvider;
        }

        /// <summary>
        /// Create document operation.
        /// </summary>
        /// <param name="command">Create document command.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost(Name = "DocumentDetails.Create")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateDocument([FromBody] CreateDocument command)
        {
            await _commandQueryProvider.SendAsync(command);
            return Created("/", new {id = command.CreatedId});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet(Name = "DocumentDetails.GetList")]
        [ProducesResponseType(typeof(IReadOnlyCollection<DocumentResponse>), 200)]
        public Task<IReadOnlyCollection<DocumentResponse>> GetList([FromQuery] GetDocuments query)
            => _commandQueryProvider.SendAsync(query);

        [HttpGet("{documentId}", Name = "DocumentDetails.GetDocument")]
        [ProducesResponseType(typeof(DocumentResponse), 200)]
        public Task<DocumentResponse> GetDocument([FromRoute] GetDocument query)
            => _commandQueryProvider.SendAsync(query);

        [HttpPatch("{documentId}/details", Name = "DocumentDetails.UpdateDetails")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateDocumentDetails()
        {
            throw new NotImplementedException();
        }

        [HttpPut("{documentId}/addresses", Name = "DocumentDetails.UpdateAddresses")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateDocumentAddresses()
        {
            throw new NotImplementedException();
        }
    }
}
