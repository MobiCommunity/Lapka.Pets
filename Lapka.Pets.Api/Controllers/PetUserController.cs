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
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
            => Ok(await _queryDispatcher.QueryAsync(new GetUserPet
            {
                Id = id
            }));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetAll()
            => Ok(await _queryDispatcher.QueryAsync(new GetUserPets()));

        /// <summary>
        /// User id has to be provided (from identity service) until
        /// auth will be requried to add pet
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateUserPetRequest pet)
        {
            if (!Guid.TryParse(pet.UserId.ToString(), out Guid userId))
            {
                return Unauthorized();
            }
            
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

            await _commandDispatcher.SendAsync(new CreateUserPet(id, userId, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.MainPhoto.AsPhotoFile(mainPhotoId), pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                photos));

            return Created($"api/pet/{id}", null);
        }

        /// <summary>
        /// User id has to be provided (from identity service) until
        /// auth will be requried to add pet visit
        /// </summary>
        [HttpPost("{id:guid}/visit")]
        public async Task<IActionResult> AddVisit(Guid id, [FromBody] AddVisitRequest request)
        {
            if (!Guid.TryParse(request.UserId.ToString(), out Guid userId))
            {
                return Unauthorized();
            }

            Guid visitId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new AddVisit(userId, id, request.AsValueObject(visitId)));

            return NoContent();
        }
        
        /// <summary>
        /// User id has to be provided (from identity service) until
        /// auth will be requried to update a visit
        /// </summary>
        
        [HttpPatch("{id:guid}/visit/{visitId:guid}")]
        public async Task<IActionResult> UpdateVisit(Guid id, Guid visitId, [FromBody] UpdateVisitRequest request)
        {
            if (!Guid.TryParse(request.UserId.ToString(), out Guid userId))
            {
                return Unauthorized();
            }
            
            await _commandDispatcher.SendAsync(new UpdateVisit(userId, id, request.AsValueObject(visitId)));

            return NoContent();
        }
        
        /// <summary>
        /// User id has to be provided (from identity service) until
        /// auth will be requried to add pet soon event
        /// </summary>

        [HttpPost("{id:guid}/soonEvent")]
        public async Task<IActionResult> AddSoonEvent(Guid id, [FromBody] AddSoonEventRequest request)
        {
            if (!Guid.TryParse(request.UserId.ToString(), out Guid userId))
            {
                return Unauthorized();
            }

            Guid soonEventId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new AddSoonEvent(userId, id, request.AsValueObject(soonEventId)));

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            string userId = User.Identity.Name;
            await _commandDispatcher.SendAsync(new DeleteUserPet(userId, id));

            return NoContent();
        }

        /// <summary>
        /// Deletes photo from Photos list (not a main photo)
        /// </summary>
        [HttpDelete("{id:guid}/photo")]
        public async Task<IActionResult> DeletePhoto(Guid id, DeletePetPhotoRequest photo)
        {
            await _commandDispatcher.SendAsync(new DeleteUserPetPhoto(id, photo.Id));

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

            await _commandDispatcher.SendAsync(new AddUserPetPhoto(id, photos));

            return Ok();
        }

        /// <summary>
        /// Updates only main pet photo
        /// </summary>
        [HttpPatch("{id:guid}/photo")]
        public async Task<IActionResult> UpdatePhoto(Guid id, [FromForm] UpdatePetPhotoRequest petUpdate)
        {
            Guid photoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new UpdateUserPetPhoto(id, petUpdate.File.AsPhotoFile(photoId)));

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateUserPetRequest pet)
        {
            await _commandDispatcher.SendAsync(new UpdateUserPet(id, pet.Name, pet.Race, pet.Species, pet.Sex,
                pet.DateOfBirth, pet.Sterilization, pet.Weight, pet.Color));

            return NoContent();
        }
    }
}