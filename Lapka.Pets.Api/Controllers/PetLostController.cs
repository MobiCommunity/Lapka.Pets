using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Pets.Api.Models.Request;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure;
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
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, string longitude, string latitude)
            => Ok(await _queryDispatcher.QueryAsync(new GetLostPet
            {
                Id = id,
                Latitude = latitude,
                Longitude = longitude
            }));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetAll(string latitude, string longitude)
            => Ok(await _queryDispatcher.QueryAsync(new GetLostPets
            {
                Latitude = latitude,
                Longitude = longitude
            }));

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateLostPetRequest pet)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            Guid id = Guid.NewGuid();
            Guid mainPhotoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new CreateLostPet(id, userId, pet.Name, pet.Race, pet.Species,
                pet.Sex, pet.Age, pet.LostDate, pet.Sterilization, pet.Weight, pet.Color, pet.OwnerName,
                pet.PhoneNumber, pet.LostAddress.AsValueObject(), pet.Description,
                pet.MainPhoto.AsPhotoFile(mainPhotoId), pet.Photos.CreatePhotoFiles()));

            return Created($"api/pet/lost/{id}", null);
        }

        /// <summary>
        /// Deletes photo from Photos list (not a main photo)
        /// </summary>
        [HttpDelete("{id:guid}/photo")]
        public async Task<IActionResult> DeletePhoto(Guid id, DeletePetPhotoRequest photo)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new DeleteLostPetPhoto(id, userId, photo.Id));

            return NoContent();
        }

        /// <summary>
        /// Adds multiple photos to pet
        /// </summary>
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

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateLostPetRequest pet)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new UpdateLostPet(id, userId, pet.Name, pet.Race, pet.Species, pet.Sex,
                pet.Age, pet.LostDate, pet.Weight, pet.Color, pet.OwnerName, pet.PhoneNumber,
                pet.LostAddress.AsValueObject(),
                pet.Description));

            return NoContent();
        }

        /// <summary>
        /// Updates only main pet photo
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

            await _commandDispatcher.SendAsync(new UpdateLostPetPhoto(id, userId, petUpdate.File.AsPhotoFile(photoId)));

            return NoContent();
        }
    }
}