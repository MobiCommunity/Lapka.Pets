using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Pets.Api.Helpers;
using Lapka.Pets.Api.Models.Request;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Core.ValueObjects;
using Lapka.Pets.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Pets.Api.Controllers
{
    [ApiController]
    [Route("api/user/pet")]
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
        /// Gets user pet by ID (auth required)
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            return Ok(await _queryDispatcher.QueryAsync(new GetUserPet
            {
                Id = id,
                UserId = userId
            }));
        }

        /// <summary>
        /// Gets all user pets (auth required)
        /// </summary>
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
        /// Add user's pet (auth required)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateUserPetRequest pet)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            Guid id = Guid.NewGuid();
            Guid mainPhotoId = Guid.NewGuid();
            List<PhotoFile> photos = PetControllerHelpers.CreatePhotoFiles(pet.Photos);

            await _commandDispatcher.SendAsync(new CreateUserPet(id, userId, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.MainPhoto.AsPhotoFile(mainPhotoId), pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                photos));

            return Created($"api/pet/{id}", null);
        }

        /// <summary>
        /// Add visit for the user's pet (auth required)
        /// </summary>
        [HttpPost("{id:guid}/visit")]
        public async Task<IActionResult> AddVisit(Guid id, [FromBody] AddVisitRequest request)
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
        /// Updates a visit for the user's pet (auth required)
        /// </summary>
        
        [HttpPatch("{id:guid}/visit/{visitId:guid}")]
        public async Task<IActionResult> UpdateVisit(Guid id, Guid visitId, [FromBody] UpdateVisitRequest request)
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
        /// Adds event for the user's pet (auth required)
        /// </summary>

        [HttpPost("{id:guid}/soonEvent")]
        public async Task<IActionResult> AddSoonEvent(Guid id, [FromBody] AddSoonEventRequest request)
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
        /// Deletes user's pet (auth required)
        /// </summary>

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
        /// Deletes photo from pet's photos list(not a main photo)(auth required)
        /// </summary>
        [HttpDelete("{id:guid}/photo")]
        public async Task<IActionResult> DeletePhoto(Guid id, DeletePetPhotoRequest photo)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            await _commandDispatcher.SendAsync(new DeleteUserPetPhoto(id, userId, photo.Id));

            return Ok();
        }
        
        /// <summary>
        /// Adds multiple photos to the pet(auth required)
        /// </summary>
        [HttpPost("{id:guid}/photo")]
        public async Task<IActionResult> AddPhotos(Guid id, [FromForm] AddPetPhotoRequest request)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            List<PhotoFile> photos = PetControllerHelpers.CreatePhotoFiles(request.Photos);
            
            await _commandDispatcher.SendAsync(new AddUserPetPhoto(id, userId, photos));

            return Ok();
        }

        /// <summary>
        /// Updates only pet's main photo(auth required)
        /// </summary>
        [HttpPatch("{id:guid}/photo")]
        public async Task<IActionResult> UpdatePhoto(Guid id, [FromForm] UpdatePetPhotoRequest petUpdate)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            Guid photoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new UpdateUserPetPhoto(id, userId, petUpdate.File.AsPhotoFile(photoId)));

            return NoContent();
        }

        /// <summary>
        /// Updates the user's pet (auth required)
        /// </summary>
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateUserPetRequest pet)
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