using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Pets.Api.Models.Request;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Pets.Api.Controllers
{
    [ApiController]
    [Route("api/pet/user")]
    public class PetUserController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public PetUserController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
            => Ok(await _queryDispatcher.QueryAsync(new GetUserPet
            {
                Id = id
            }));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetAll()
            => Ok(await _queryDispatcher.QueryAsync(new GetUserPets()));

        [HttpPost("user")]
        public async Task<IActionResult> Add([FromForm] CreateUserPetRequest pet)
        {
            string? userId = User.Identity.Name;
            Guid id = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new CreateUserPet(id, userId, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.File.AsValueObject(), pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization));

            return Created($"api/pet/{id}", null);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _commandDispatcher.SendAsync(new DeletePet(id));

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateUserPetRequest pet)
        {
            await _commandDispatcher.SendAsync(new UpdateUserPet(id, pet.Name, pet.Race, pet.Species,
                pet.File.AsValueObject(), pet.Sex, pet.DateOfBirth, pet.Sterilization, pet.Weight, pet.Color));

            return NoContent();
        }
    }
}