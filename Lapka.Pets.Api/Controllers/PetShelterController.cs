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
using Lapka.Pets.Core.ValueObjects;
using Lapka.Pets.Infrastructure;
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
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();

            Guid id = Guid.NewGuid();
            Guid mainPhotoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new CreateShelterPet(id, userId, pet.Name, pet.Sex, pet.Race,
                pet.Species, pet.MainPhoto.AsPhotoFile(mainPhotoId), pet.BirthDay, pet.Color, pet.Weight,
                pet.Sterilization, pet.ShelterId, pet.ShelterAddress.AsValueObject(), pet.Description,
                pet.Photos.CreatePhotoFiles()));

            return Created($"api/pet/shelter/{id}", null);
        }

        /// <summary>
        /// Deletes photo from Photos list (not a main photo)
        /// </summary>
        [HttpDelete("{id:guid}/photo")]
        public async Task<IActionResult> DeletePhoto(Guid id, DeletePetPhotoRequest photo)
        {
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (string.IsNullOrEmpty(userRole) || userRole != "shelter")
            {
                return Unauthorized();
            }

            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();

            await _commandDispatcher.SendAsync(new DeleteShelterPetPhoto(id, userId, photo.Id));

            return NoContent();
        }

        /// <summary>
        /// Adds multiple photos to pet
        /// </summary>
        [HttpPost("{id:guid}/photo")]
        public async Task<IActionResult> AddPhotos(Guid id, [FromForm] AddPetPhotoRequest request)
        {
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (string.IsNullOrEmpty(userRole) || userRole != "shelter")
            {
                return Unauthorized();
            }

            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();

            await _commandDispatcher.SendAsync(new AddShelterPetPhoto(id, userId, request.Photos.CreatePhotoFiles()));

            return NoContent();
        }

        /// <summary>
        /// Adds multiple photos to pet
        /// </summary>
        [HttpPost("stray")]
        public async Task<IActionResult> ReportStrayPet([FromForm] ReportStrayPetRequest request)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (Guid.Empty == userId)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new ReportStrayPet(userId, request.Location.AsValueObject(),
                request.Photos.CreatePhotoFiles(), request.Description, request.ReporterName,
                request.ReporterPhoneNumber));

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (string.IsNullOrEmpty(userRole) || userRole != "shelter")
            {
                return Unauthorized();
            }

            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();

            await _commandDispatcher.SendAsync(new DeleteShelterPet(id, userId));

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateShelterPetRequest pet)
        {
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (string.IsNullOrEmpty(userRole) || userRole != "shelter")
            {
                return Unauthorized();
            }

            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();

            await _commandDispatcher.SendAsync(new UpdateShelterPet(id, userId, pet.Name, pet.Race, pet.Species,
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
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (string.IsNullOrEmpty(userRole) || userRole != "shelter")
            {
                return Unauthorized();
            }

            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            Guid photoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new UpdateShelterPetPhoto(id, userId,
                petUpdate.File.AsPhotoFile(photoId)));

            return NoContent();
        }
    }
}