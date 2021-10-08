using System;
using System.Globalization;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Grpc.Core;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Queries.ShelterPets;

namespace Lapka.Pets.Api.Grpc.Controllers
{
    public class GrpcPetController : PetProto.PetProtoBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public GrpcPetController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        public override async Task<GetPetsShelterReply> GetPetsShelter(GetPetsShelterRequest request,
            ServerCallContext context)
        {
            if (!Guid.TryParse(request.PetId, out Guid petId))
            {
                throw new InvalidPetIdException(request.PetId);
            }

            PetDetailsShelterDto pet = await _queryDispatcher.QueryAsync(new GetShelterPetMongo
            {
                Id = petId
            });
            return new GetPetsShelterReply
            {
                ShelterId = pet?.ShelterId.ToString() ?? string.Empty
            };
        }

        public override async Task<GetShelterPetBasicInfoReply> GetShelterPetBasicInfo(
            GetShelterPetBasicInfoRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.PetId, out Guid petId))
            {
                throw new InvalidPetIdException(request.PetId);
            }

            PetDetailsShelterDto pet = await _queryDispatcher.QueryAsync(new GetShelterPetMongo
            {
                Id = petId
            });
            return new GetShelterPetBasicInfoReply
            {
                PetName = pet?.Name,
                Race = pet?.Race,
                BirthDate = pet?.BirthDay.ToString(CultureInfo.InvariantCulture),
                PhotoPaths = {pet?.PhotoPaths}
            };
        }
    }
}