using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Lapka.Pets.Api.Models.Request;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Queries;

namespace Lapka.Pets.Api.Controllers
{
    [ApiController]
    [Route("api/values")]
    public class ValuesController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ValuesController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetValue
            {
                Id = id
            }));
        }

        [HttpPost]
        public async Task<ActionResult> Add(CreateValueRequest valueRequest)
        {
            Guid id = Guid.NewGuid();
            await _commandDispatcher.SendAsync(new CreateValue(valueRequest.Name,valueRequest.Description,id));

            return Created($"api/lapka.pets/values/{id}", null);
        }
    }
}