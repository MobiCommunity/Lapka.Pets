using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Pets.Api.Models.Request;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.UserPets;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Queries.UserPets;
using Lapka.Pets.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Pets.Api.Controllers
{
    [ApiController]
    [Route("api/pets/user/pet")]
    public class PetUserController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public PetUserController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }
        
        /// <summary>
        /// Gets user pet by ID. User has to be logged.
        /// </summary>
        /// <returns>Get user pet</returns>
        /// <response code="200">If successfully got user pet</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If pet does not belong to user</response>
        /// <response code="404">If pet is not found</response>
        [ProducesResponseType(typeof(PetDetailsUserDto), StatusCodes.Status200OK)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            return Ok(await _queryDispatcher.QueryAsync(new GetUserPetElastic
            {
                Id = id,
                UserId = userId
            }));
        }
        
        /// <summary>
        /// Gets user pets. User has to be logged.
        /// </summary>
        /// <returns>Get all user pets</returns>
        /// <response code="200">If successfully got lost pets</response>
        /// <response code="401">If user is not logged</response>
        [ProducesResponseType(typeof(IEnumerable<PetBasicUserDto>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetAll()
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            return Ok(await _queryDispatcher.QueryAsync(new GetUserPets
            {
                UserId = userId
            }));
        }
        
        /// <summary>
        /// Adds user pet. User has to be logged.
        /// </summary>
        /// <returns>URL to added pet</returns>
        /// <response code="200">If successfully added user pet</response>
        /// <response code="400">If given properties were invalid</response>
        /// <response code="401">If user is not logged</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateUserPetRequest pet)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            Guid id = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new CreateUserPet(id, userId, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.MainPhoto.AsPhotoFile(), pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                pet.Photos.CreatePhotoFiles()));

            return Created($"api/pet/{id}", null);
        }
        
        /// <summary>
        /// Adds last visit to pet. User has to be logged.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">If successfully added last visit to pet</response>
        /// <response code="400">If given properties were invalid</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If pet does not belong to user</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpPost("{id:guid}/visit")]
        public async Task<IActionResult> AddVisit(Guid id, AddVisitRequest request)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            Guid visitId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new AddVisit(userId, id, request.AsValueObject(visitId)));

            return NoContent();
        }

        /// <summary>
        /// Updates last visit to pet. User has to be logged.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">If successfully updated last visit to pet</response>
        /// <response code="400">If given properties were invalid</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If pet does not belong to user</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpPatch("{id:guid}/visit/{visitId:guid}")]
        public async Task<IActionResult> UpdateVisit(Guid id, Guid visitId, UpdateVisitRequest request)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            await _commandDispatcher.SendAsync(new UpdateVisit(userId, id, request.AsValueObject(visitId)));

            return NoContent();
        }

        /// <summary>
        /// Adds soon event to pet. User has to be logged.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">If successfully added soon event to pet</response>
        /// <response code="400">If given properties were invalid</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If pet does not belong to user</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpPost("{id:guid}/soonEvent")]
        public async Task<IActionResult> AddSoonEvent(Guid id, AddSoonEventRequest request)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            Guid soonEventId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new AddSoonEvent(userId, id, request.AsValueObject(soonEventId)));

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
            
            await _commandDispatcher.SendAsync(new DeleteUserPet(userId, id));

            return NoContent();
        }
        
        /// <summary>
        /// Deletes photos from user pet photos list. User has to be logged and owner of pet.
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
            
            await _commandDispatcher.SendAsync(new DeleteUserPetPhoto(id, userId, photo.Path));

            return NoContent();
        }
        
        /// <summary>
        /// Adds photos to user pet photos list. User has to be logged and be owner of pet.
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
            
            await _commandDispatcher.SendAsync(new AddUserPetPhoto(id, userId, request.Photos.CreatePhotoFiles()));

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
            
            Guid photoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new UpdateUserPetPhoto(id, userId, petUpdate.File.AsPhotoFile()));

            return NoContent();
        }
        
        /// <summary>
        /// Updates a user pet. User has to be logged and be owner of pet.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If successfully updated pet</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not owner of pet</response>
        /// <response code="404">If pet is not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserPetRequest pet)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            await _commandDispatcher.SendAsync(new UpdateUserPet(id, userId, pet.Name, pet.Race, pet.Species, pet.Sex,
                pet.DateOfBirth, pet.Sterilization, pet.Weight, pet.Color));

            return NoContent();
        }
    }
}