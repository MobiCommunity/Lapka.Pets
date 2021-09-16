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

            await _commandDispatcher.SendAsync(new CreateUserPet(id, userId, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.MainPhoto.AsPhotoFile(mainPhotoId), pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                pet.Photos.CreatePhotoFiles()));

            return Created($"api/pet/{id}", null);
        }
        
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
        
        [HttpDelete("{id:guid}/photo")]
        public async Task<IActionResult> DeletePhoto(Guid id, DeletePetPhotoRequest photo)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            await _commandDispatcher.SendAsync(new DeleteUserPetPhoto(id, userId, photo.Id));

            return NoContent();
        }
        
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