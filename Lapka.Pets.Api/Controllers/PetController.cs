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
    [Route("api/pet")]
    public class PetController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public PetController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetPet
            {
                Id = id
            }));
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetAll()
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetPets()));
        }

        [HttpGet("{race}")]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetByRace(string race)
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetPetsByRace
            {
                Race = race
            }));
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreatePetRequest pet)
        {
            Guid id = Guid.NewGuid();
            await _commandDispatcher.SendAsync(new CreatePet(id, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.File.AsValueObject(),
                pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress.AsValueObject(),
                pet.Description));

            return Created($"api/pet/{id}", null);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _commandDispatcher.SendAsync(new DeletePet(id));

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdatePetRequest petUpdate)
        {
            await _commandDispatcher.SendAsync(new UpdatePet(id, petUpdate.Name, petUpdate.Race,
                petUpdate.Species, petUpdate.File.AsValueObject(), petUpdate.Sex, petUpdate.DateOfBirth,
                petUpdate.Description,
                petUpdate.ShelterAddress.AsValueObject(), petUpdate.Sterilization, petUpdate.Weight,
                petUpdate.Color));

            return NoContent();
        }
    }
}