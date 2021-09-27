using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.ShelterPets;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Pets.Api.Controllers
{
    [ApiController]
    [Route("api/pets/shelter/pet")]
    public class PetShelterLikeController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public PetShelterLikeController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }
        
        /// <summary>
        /// Likes a shelter pets. User has to be logged.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If successfully liked pet</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="404">If pet is not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
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
        
        /// <summary>
        /// Dislikes a shelter pets. User has to be logged.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If successfully disliked pet</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="404">If pet is not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
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
        
        /// <summary>
        /// Gets all user liked pets. User has to be logged.
        /// </summary>
        /// <returns>Liked pets</returns>
        /// <response code="200">If successfully got liked pets</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="404">If pet is not found</response>
        [ProducesResponseType(typeof(IEnumerable<PetBasicShelterDto>), StatusCodes.Status200OK)]
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