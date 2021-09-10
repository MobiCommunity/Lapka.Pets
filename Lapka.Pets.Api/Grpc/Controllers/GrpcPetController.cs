using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Grpc.Core;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;

namespace Lapka.Identity.Api.Grpc.Controllers
{
    public class GrpcPetController : PetProto.PetProtoBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public GrpcPetController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }
        public override async Task<GetShelterPetsCountReply> GetShelterPetsCount(GetShelterPetsCountRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.ShelterId, out Guid shelterId))
            {
                throw new InvalidShelterIdException(request.ShelterId);
            }
            
            int response = await _queryDispatcher.QueryAsync(new GetShelterPetCount
            {
                ShelterId = shelterId
            });

            return new GetShelterPetsCountReply
            {
                Count = response
            };
        }

        public override async Task<DoesPetExistsReply> DoesPetExists(DoesPetExistsRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.PetId, out Guid petId))
            {
                throw new InvalidPetIdException(request.PetId);
            }
            
            PetDetailsShelterDto pet = await _queryDispatcher.QueryAsync(new GetShelterPet
            {
                Id = petId
            });
            
            return new DoesPetExistsReply
            {
                DoesExists = pet is { }
            };
        }
    }
}