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
        public async Task<IActionResult> Add([FromForm] CreateShelterPetRequest shelterPet)
        {
            string? userId = User.Identity.Name;
            Guid id = Guid.NewGuid();
            Guid photoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new CreateShelterPet(id, userId, shelterPet.Name, shelterPet.Sex,
                shelterPet.Race, shelterPet.Species, shelterPet.File.AsValueObject(), shelterPet.BirthDay,
                shelterPet.Color, shelterPet.Weight, shelterPet.Sterilization,
                shelterPet.ShelterAddress.AsValueObject(), shelterPet.Description, photoId));

            return Created($"api/pet/shelter/{id}", null);
        }
        
        [HttpDelete("{id:guid}/photo")]
        public async Task<IActionResult> DeletePhoto(Guid id, DeletePetPhotoRequest photo)
        {
            await _commandDispatcher.SendAsync(new DeleteShelterPetPhoto(id, photo.Path));

            return Ok();
        }
        
        [HttpPost("{id:guid}/photo")]
        public async Task<IActionResult> AddPhotos(Guid id, [FromForm] AddPetPhotoRequest photos)
        {
            await _commandDispatcher.SendAsync(new AddShelterPetPhoto(id,
                photos.Photos.Select(x => x.AsValueObject()).ToList()));

            return Ok();
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _commandDispatcher.SendAsync(new DeleteShelterPet(id));

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateShelterPetRequest pet)
        {
            await _commandDispatcher.SendAsync(new UpdateShelterPet(id, pet.Name, pet.Race, pet.Species,
                pet.Sex, pet.DateOfBirth, pet.Description, pet.ShelterAddress.AsValueObject(),
                pet.Sterilization, pet.Weight, pet.Color));

            return NoContent();
        }
        
        [HttpPatch("{id:guid}/photo")]
        public async Task<IActionResult> UpdatePhoto(Guid id, [FromForm] UpdatePetPhotoRequest petUpdate)
        {
            Guid photoId = Guid.NewGuid();
            
            await _commandDispatcher.SendAsync(new UpdateShelterPetPhoto(id, petUpdate.File.AsValueObject(), photoId));

            return NoContent();
        }
    }
}