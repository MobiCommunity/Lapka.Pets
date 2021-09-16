using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.ShelterPets;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Pets.Api.Controllers
{
    [ApiController]
    [Route("api/shelter/pet")]
    public class PetShelterLikeController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public PetShelterLikeController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }
        
        [HttpPatch("{id:guid}/like")]
        public async Task<IActionResult> LikePet(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new LikePet(id, userId));

            return NoContent();
        }
        
        [HttpPatch("{id:guid}/dislike")]
        public async Task<IActionResult> DislikePet(Guid id)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new DislikePet(id, userId));

            return NoContent();
        }
        
        [HttpGet("like")]
        public async Task<IActionResult> GetLikedPets(string longitude, string latitude)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            return Ok(await _queryDispatcher.QueryAsync(new GetLikedPets
            {
                UserId = userId,
                Longitude = longitude,
                Latitude = latitude
            }));
        }
    }
}