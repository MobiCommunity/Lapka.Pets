using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Queries;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetUserPetsHandler : IQueryHandler<GetUserPets, IEnumerable<PetBasicUserDto>>
    {
        public Task<IEnumerable<PetBasicUserDto>> HandleAsync(GetUserPets query)
        {
            throw new System.NotImplementedException();
        }
    }
}