using MasterPerform.Infrastructure.Messaging.Handlers;
using MasterPerform.WebApi.Contracts;
using MasterPerform.WebApi.Utilities.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MasterPerform.WebApi.Controllers
{
    [MasterPerformRoute("/document")]
    public class DocumentController : Controller
    {
        private readonly ICommandHandler<CreateDocument> commandHandler;

        public DocumentController(ICommandHandler<CreateDocument> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateDocument(CreateDocument command)
        {
            await commandHandler.Handler(command);
            return Created("/", new {id = command.CreatedId});
        }
    }
}
