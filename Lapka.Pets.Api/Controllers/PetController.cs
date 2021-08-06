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
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetPets()
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetPets()));
        }
        
        [HttpGet("{race}")]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetPetsByRace(string race)
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetPetsByRace
            {
                Race = race
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreatePetRequest pet)
        {
            var id = Guid.NewGuid();
            await _commandDispatcher.SendAsync(new CreatePet(id, pet.Name, pet.Sex, pet.Species, pet.Race,
                pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress.AsValueObject(), pet.Description));

            return Created($"api/pet/{id}", null);
        }
    }
}