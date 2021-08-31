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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Pets.Api.Controllers
{
    [ApiController]
    [Route("api/shelter/pet")]
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
        public async Task<IActionResult> Add([FromForm] CreateShelterPetRequest pet)
        {
            string userId = User.Identity.Name;
            Guid id = Guid.NewGuid();
            Guid mainPhotoId = Guid.NewGuid();
            List<PhotoFile> photos = new List<PhotoFile>();

            if (pet.Photos != null)
            {
                foreach (IFormFile photo in pet.Photos)
                {
                    photos.Add(photo.AsPhotoFile(Guid.NewGuid()));
                }
            }
            
            await _commandDispatcher.SendAsync(new CreateShelterPet(id, userId, pet.Name, pet.Sex,
                pet.Race, pet.Species, pet.MainPhoto.AsPhotoFile(mainPhotoId), pet.BirthDay,
                pet.Color, pet.Weight, pet.Sterilization,
                pet.ShelterAddress.AsValueObject(), pet.Description, photos));

            return Created($"api/pet/shelter/{id}", null);
        }

        /// <summary>
        /// Deletes photo from Photos list (not a main photo)
        /// </summary>
        [HttpDelete("{id:guid}/photo")]
        public async Task<IActionResult> DeletePhoto(Guid id, DeletePetPhotoRequest photo)
        {
            await _commandDispatcher.SendAsync(new DeleteShelterPetPhoto(id, photo.Id));

            return Ok();
        }
        
        /// <summary>
        /// Adds multiple photos to pet
        /// </summary>
        [HttpPost("{id:guid}/photo")]
        public async Task<IActionResult> AddPhotos(Guid id, [FromForm] AddPetPhotoRequest request)
        {
            List<PhotoFile> photos = new List<PhotoFile>();
            
            foreach (IFormFile photo in request.Photos)
            {
                photos.Add(photo.AsPhotoFile(Guid.NewGuid()));
            }
            
            await _commandDispatcher.SendAsync(new AddShelterPetPhoto(id, photos));

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

        /// <summary>
        /// Updates only main pet photo
        /// </summary>
        [HttpPatch("{id:guid}/photo")]
        public async Task<IActionResult> UpdatePhoto(Guid id, [FromForm] UpdatePetPhotoRequest petUpdate)
        {
            Guid photoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new UpdateShelterPetPhoto(id, petUpdate.File.AsPhotoFile(photoId)));

            return NoContent();
        }
    }
}