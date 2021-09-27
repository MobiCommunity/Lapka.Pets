using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Pets.Api.Models.Request;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.ShelterPets;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Queries.ShelterPets;
using Lapka.Pets.Core.ValueObjects;
using Lapka.Pets.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Pets.Api.Controllers
{
    [ApiController]
    [Route("api/pets/shelter")]
    public class PetShelterController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public PetShelterController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Gets shelter pet by ID.
        /// </summary>
        /// <returns>Get shelter pet</returns>
        /// <response code="200">If successfully got shelter pet</response>
        /// <response code="404">If pet is not found</response>
        [ProducesResponseType(typeof(PetDetailsShelterDto), StatusCodes.Status200OK)]
        [HttpGet("pet/{id:guid}")]
        public async Task<IActionResult> Get(Guid id, string longitude, string latitude)
            => Ok(await _queryDispatcher.QueryAsync(new GetShelterPetElastic
            {
                Id = id,
                Latitude = latitude,
                Longitude = longitude
            }));

        
        /// <summary>
        /// Gets shelter pets.
        /// </summary>
        /// <returns>Get shelter pets</returns>
        /// <response code="200">If successfully got shelter pets</response>
        [ProducesResponseType(typeof(IEnumerable<PetBasicShelterDto>), StatusCodes.Status200OK)]
        [HttpGet("pet")]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetAll(string name, string race,
            string latitude, string longitude)
            => Ok(await _queryDispatcher.QueryAsync(new GetShelterPets
            {
                Latitude = latitude,
                Longitude = longitude,
                Name = name,
                Race = race
            }));
        
        /// <summary>
        /// Gets all pets belong to given shelter.
        /// </summary>
        /// <returns>Gets all pets which belong to given shelter</returns>
        /// <response code="200">If successfully got shelter own pets</response>
        [ProducesResponseType(typeof(IEnumerable<PetBasicDto>), StatusCodes.Status200OK)]
        [HttpGet("{id:guid}/pet")]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetAllShelterPets(Guid id)
            => Ok(await _queryDispatcher.QueryAsync(new GetShelterOwnPets
            {
                ShelterId = id
            }));
        
        /// <summary>
        /// Gets total pet count belong to given shelter.
        /// </summary>
        /// <returns>Gets total count of pets that belong to given shelter</returns>
        /// <response code="200">If successfully got shelter pet count</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [HttpGet("{id:guid}/pet/count")]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetShelterPetCount(Guid id)
            => Ok(await _queryDispatcher.QueryAsync(new GetShelterOwnPetsCount
            {
                ShelterId = id
            }));

        /// <summary>
        /// Adds shelter pet. User has to be logged, and owner of shelter to which want add a pet.
        /// </summary>
        /// <returns>URL to added pet</returns>
        /// <response code="200">If successfully added shelter pet</response>
        /// <response code="400">If given properties were invalid</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not owner of shelter</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [HttpPost("pet")]
        public async Task<IActionResult> Add([FromForm] CreateShelterPetRequest pet)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();

            Guid id = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new CreateShelterPet(id, userId, pet.Name, pet.Sex, pet.Race,
                pet.Species, pet.MainPhoto.AsPhotoFile(), pet.BirthDay, pet.Color, pet.Weight,
                pet.Sterilization, pet.Description, pet.ShelterId, pet.Photos.CreatePhotoFiles()));

            return Created($"api/pet/shelter/{id}", null);
        }

        /// <summary>
        /// Deletes photos from shelter pet photos list. User has to be logged and owner of shelter.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">If successfully deleted photos</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not owner of shelter</response>
        /// <response code="404">If pet or photo is not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpDelete("pet/{id:guid}/photo")]
        public async Task<IActionResult> DeletePhotos(Guid id, DeletePetPhotoRequest photo)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (Guid.Empty == userId)
            {
                return Unauthorized();
            }
            
            await _commandDispatcher.SendAsync(new DeleteShelterPetPhoto(id, userId, photo.Path));

            return NoContent();
        }

        /// <summary>
        /// Adds photos to shelter pet photos list. User has to be logged and be owner of shelter.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">If successfully added photos</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not owner of shelter</response>
        /// <response code="404">If pet is not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpPost("pet/{id:guid}/photo")]
        public async Task<IActionResult> AddPhotos(Guid id, [FromForm] AddPetPhotoRequest request)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (Guid.Empty == userId)
            {
                return Unauthorized();
            }
            
            await _commandDispatcher.SendAsync(new AddShelterPetPhoto(id, userId, request.Photos.CreatePhotoFiles()));

            return NoContent();
        }

        /// <summary>
        /// Soft deletes pet. User has to be logged and owner of shelter.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If successfully deleted pet</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not owner of shelter</response>
        /// <response code="404">If pet is not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpDelete("pet/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (Guid.Empty == userId)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new DeleteShelterPet(id, userId));

            return NoContent();
        }

        /// <summary>
        /// Updates a shelter pet. User has to be logged and be owner of shelter.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If successfully updated pet</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not owner of shelter</response>
        /// <response code="404">If pet is not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpPatch("pet/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateShelterPetRequest pet)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (Guid.Empty == userId)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new UpdateShelterPet(id, userId, pet.Name, pet.Race, pet.Species,
                pet.Sex, pet.DateOfBirth, pet.Description, pet.Sterilization, pet.Weight, pet.Color));

            return NoContent();
        }

        /// <summary>
        /// Updates main photo. User has to be logged and be owner of shelter.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If successfully updated pet photo</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not owner of shelter</response>
        /// <response code="404">If pet is not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpPatch("pet/{id:guid}/photo")]
        public async Task<IActionResult> UpdatePhoto(Guid id, [FromForm] UpdatePetPhotoRequest petUpdate)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (Guid.Empty == userId)
            {
                return Unauthorized();
            }
            
            Guid photoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new UpdateShelterPetPhoto(id, userId,
                petUpdate.File.AsPhotoFile()));

            return NoContent();
        }
    }
}