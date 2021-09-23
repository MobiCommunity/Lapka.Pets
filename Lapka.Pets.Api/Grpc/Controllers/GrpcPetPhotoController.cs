using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Grpc.Core;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Queries.LostPets;
using Lapka.Pets.Application.Queries.ShelterPets;
using Lapka.Pets.Application.Queries.UserPets;

namespace Lapka.Pets.Api.Grpc.Controllers
{
    public class GrpcPetPhotoController : PetPhotosProto.PetPhotosProtoBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public GrpcPetPhotoController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        public override async Task<GetLostPetPhotosReply> GetLostPetPhotos(GetLostPetPhotosRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.PetId, out Guid petId))
            {
                throw new InvalidShelterIdException(request.PetId);
            }
            
            PetDetailsLostDto response = await _queryDispatcher.QueryAsync(new GetLostPetMongo
            {
                Id = petId
            });

            Collection<string> photoPaths = new Collection<string>();
            foreach (string photo in response.PhotoPaths)
            {
                photoPaths.Add(photo);
            }
            photoPaths.Add(response.MainPhotoPath);

            return new GetLostPetPhotosReply
            {
                Paths = {photoPaths}
            };
        }

        public override async Task<GetShelterPetPhotosReply> GetShelterPetPhotos(GetShelterPetPhotosRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.PetId, out Guid petId))
            {
                throw new InvalidShelterIdException(request.PetId);
            }
            
            PetDetailsShelterDto response = await _queryDispatcher.QueryAsync(new GetShelterPetMongo
            {
                Id = petId
            });

            Collection<string> photoPaths = new Collection<string>();
            foreach (string photo in response.PhotoPaths)
            {
                photoPaths.Add(photo);
            }
            photoPaths.Add(response.MainPhotoPath);

            return new GetShelterPetPhotosReply
            {
                Paths = {photoPaths}
            };
        }

        public override async Task<GetUserPetPhotosReply> GetUserPetPhotos(GetUserPetPhotosRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.PetId, out Guid petId))
            {
                throw new InvalidShelterIdException(request.PetId);
            }
            
            PetDetailsUserDto response = await _queryDispatcher.QueryAsync(new GetUserPetMongo
            {
                Id = petId
            });

            Collection<string> photoPaths = new Collection<string>();
            foreach (string photo in response.PhotoPaths)
            {
                photoPaths.Add(photo);
            }
            photoPaths.Add(response.MainPhotoPath);

            return new GetUserPetPhotosReply
            {
                Paths = {photoPaths}
            };
        }
    }
}