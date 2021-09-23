using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Pets.Api.Models.Request;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.LostPets;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Queries.LostPets;
using Lapka.Pets.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Pets.Api.Controllers
{
    [ApiController]
    [Route("api/lost/pet")]
    public class PetLostController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        
        public PetLostController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }
        
        /// <summary>
        /// Gets lost pet by ID.
        /// </summary>
        /// <returns>Get lost pet</returns>
        /// <response code="200">If successfully got lost pet</response>
        /// <response code="404">If pet is not found</response>
        [ProducesResponseType(typeof(PetDetailsLostDto), StatusCodes.Status200OK)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, string longitude, string latitude)
            => Ok(await _queryDispatcher.QueryAsync(new GetLostPetElastic
            {
                Id = id,
                Latitude = latitude,
                Longitude = longitude
            }));

        /// <summary>
        /// Gets lost pets.
        /// </summary>
        /// <returns>Get lost pets</returns>
        /// <response code="200">If successfully got lost pets</response>
        [ProducesResponseType(typeof(IEnumerable<PetBasicLostDto>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetAll(string latitude, string longitude)
            => Ok(await _queryDispatcher.QueryAsync(new GetLostPets
            {
                Latitude = latitude,
                Longitude = longitude
            }));

        /// <summary>
        /// Adds lost pet. User has to be logged
        /// </summary>
        /// <returns>URL to added pet</returns>
        /// <response code="200">If successfully added lost pet</response>
        /// <response code="400">If given properties were invalid</response>
        /// <response code="401">If user is not logged</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateLostPetRequest pet)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            Guid id = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new CreateLostPet(id, userId, pet.Name, pet.Race, pet.Species,
                pet.Sex, pet.Age, pet.LostDate, pet.Sterilization, pet.Weight, pet.Color, pet.OwnerName,
                pet.PhoneNumber.AsValueObject(), pet.LostAddress.AsValueObject(), pet.Description,
                pet.MainPhoto.AsPhotoFile(), pet.Photos.CreatePhotoFiles()));

            return Created($"api/pet/lost/{id}", null);
        }
        
        /// <summary>
        /// Deletes photos from lost pet photos list. User has to be logged and owner of pet.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">If successfully deleted photos</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not owner of pet</response>
        /// <response code="404">If pet or photo is not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpDelete("{id:guid}/photo")]
        public async Task<IActionResult> DeletePhotos(Guid id, DeletePetPhotoRequest photo)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new DeleteLostPetPhoto(id, userId, photo.Path));

            return NoContent();
        }

        /// <summary>
        /// Adds photos to lost pet photos list. User has to be logged and be owner of pet.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">If successfully added photos</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="404">If pet is not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpPost("{id:guid}/photo")]
        public async Task<IActionResult> AddPhotos(Guid id, [FromForm] AddPetPhotoRequest request)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new AddLostPetPhoto(id, userId, request.Photos.CreatePhotoFiles()));

            return NoContent();
        }

        /// <summary>
        /// Soft deletes pet. User has to be logged and owner of pet.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If successfully deleted pet</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not owner of pet</response>
        /// <response code="404">If pet is not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new DeleteLostPet(id, userId));

            return NoContent();
        }
        
        /// <summary>
        /// Updates a lost pet. User has to be logged and be owner of pet.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If successfully updated pet</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not owner of pet</response>
        /// <response code="404">If pet is not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateLostPetRequest pet)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new UpdateLostPet(id, userId, pet.Name, pet.Race, pet.Species, pet.Sex,
                pet.Age, pet.LostDate, pet.Weight, pet.Color, pet.OwnerName, pet.PhoneNumber.AsValueObject(),
                pet.LostAddress.AsValueObject(),
                pet.Description));

            return NoContent();
        }

        /// <summary>
        /// Updates main photo. User has to be logged and be owner of pet.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If successfully updated pet photo</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not owner of pet</response>
        /// <response code="404">If pet is not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpPatch("{id:guid}/photo")]
        public async Task<IActionResult> UpdatePhoto(Guid id, [FromForm] UpdatePetPhotoRequest petUpdate)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            await _commandDispatcher.SendAsync(new UpdateLostPetPhoto(id, userId, petUpdate.File.AsPhotoFile()));

            return NoContent();
        }
    }
}