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
using Lapka.Pets.Core.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Pets.Api.Controllers
{
    [ApiController]
    [Route("api/pet/shelter")]
    public class PetShelterController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public PetShelterController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, string longitude, string latitude)
            => Ok(await _queryDispatcher.QueryAsync(new GetShelterPet
            {
                Id = id,
                Latitude = latitude,
                Longitude = longitude
            }));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetAll(string name, string race,
            string latitude, string longitude)
            => Ok(await _queryDispatcher.QueryAsync(new GetShelterPets
            {
                Latitude = latitude,
                Longitude = longitude,
                Name = name,
                Race = race
            }));
        
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreatePetRequest pet)
        {
            Guid id = Guid.NewGuid();
            await _commandDispatcher.SendAsync(new CreateShelterPet(id, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.File.AsValueObject(), pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                pet.ShelterAddress.AsValueObject(), pet.Description));

            return Created($"api/pet/shelter/{id}", null);
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
            await _commandDispatcher.SendAsync(new UpdateShelterPet(id, petUpdate.Name, petUpdate.Race,
                petUpdate.Species, petUpdate.File.AsValueObject(), petUpdate.Sex, petUpdate.DateOfBirth,
                petUpdate.Description, petUpdate.ShelterAddress.AsValueObject(), petUpdate.Sterilization, petUpdate.Weight,
                petUpdate.Color));

            return NoContent();
        }
    }
}