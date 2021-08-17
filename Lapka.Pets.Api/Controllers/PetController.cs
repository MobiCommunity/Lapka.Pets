using System;
using System.Collections.Generic;
using System.Linq;
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
            => Ok(await _queryDispatcher.QueryAsync(new GetPet
            {
                Id = id
            }));
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetAll() =>
            Ok(await _queryDispatcher.QueryAsync(new GetPets()));
        

        [HttpGet("{race}")]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetByRace(string race) 
            => Ok(await _queryDispatcher.QueryAsync(new GetPetsByRace
            {
                Race = race
            }));
        
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreatePetRequest pet)
        {
            Guid id = Guid.NewGuid();
            Guid photoId = Guid.NewGuid();
            
            await _commandDispatcher.SendAsync(new CreatePet(id, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.File.AsValueObject(),
                pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress.AsValueObject(),
                pet.Description, photoId));

            return Created($"api/pet/{id}", null);
        }
        
        [HttpPost("photo/{id:guid}")]
        public async Task<IActionResult> AddPhotos(Guid id, [FromForm] AddPetPhotoRequest photos)
        {
            await _commandDispatcher.SendAsync(new AddPetPhoto(id,
                photos.Photos.Select(x => x.AsValueObject()).ToList()));

            return Ok();
        }
        
        [HttpDelete("photo/{id:guid}")]
        public async Task<IActionResult> DeletePhoto(Guid id, DeletePetPhotoRequest photo)
        {
            await _commandDispatcher.SendAsync(new DeletePetPhoto(id, photo.Path));

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _commandDispatcher.SendAsync(new DeletePet(id));

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdatePetRequest petUpdate)
        {
            await _commandDispatcher.SendAsync(new UpdatePet(id, petUpdate.Name, petUpdate.Race,
                petUpdate.Species, petUpdate.Sex, petUpdate.DateOfBirth,
                petUpdate.Description, petUpdate.ShelterAddress.AsValueObject(), petUpdate.Sterilization, petUpdate.Weight,
                petUpdate.Color));

            return NoContent();
        }
        
        [HttpPatch("photo/{id:guid}")]
        public async Task<IActionResult> UpdatePhoto(Guid id, [FromForm] UpdatePetPhotoRequest petUpdate)
        {
            Guid photoId = Guid.NewGuid();
            
            await _commandDispatcher.SendAsync(new UpdatePetPhoto(id, petUpdate.File.AsValueObject(), photoId));

            return NoContent();
        }
    }
}