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
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Pets.Api.Controllers
{
    [ApiController]
    [Route("api/shelter")]
    public class PetShelterController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public PetShelterController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("pet/{id:guid}")]
        public async Task<IActionResult> Get(Guid id, string longitude, string latitude)
            => Ok(await _queryDispatcher.QueryAsync(new GetShelterPetElastic
            {
                Id = id,
                Latitude = latitude,
                Longitude = longitude
            }));

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
        
        [HttpGet("{id:guid}/pet")]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetAllShelterPets(Guid id)
            => Ok(await _queryDispatcher.QueryAsync(new GetShelterOwnPets
            {
                ShelterId = id
            }));
        
        [HttpGet("{id:guid}/pet/count")]
        public async Task<ActionResult<IEnumerable<PetBasicDto>>> GetShelterPetCount(Guid id)
            => Ok(await _queryDispatcher.QueryAsync(new GetShelterOwnPetsCount
            {
                ShelterId = id
            }));

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
        /// Deletes photo from Photos list (not a main photo)
        /// </summary>
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
        /// Adds multiple photos to pet
        /// </summary>
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

        [HttpPatch("pet/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateShelterPetRequest pet)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (Guid.Empty == userId)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new UpdateShelterPet(id, userId, pet.Name, pet.Race, pet.Species,
                pet.Sex, pet.DateOfBirth, pet.Description, pet.ShelterAddress.AsValueObject(),
                pet.Sterilization, pet.Weight, pet.Color));

            return NoContent();
        }

        /// <summary>
        /// Updates only main pet photo
        /// </summary>
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