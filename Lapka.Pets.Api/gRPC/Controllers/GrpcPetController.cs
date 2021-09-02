using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Grpc.Core;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Exceptions;

namespace Lapka.Pets.Api.gRPC.Controllers
{
    public class GrpcPetController : PetProto.PetProtoBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        
        public GrpcPetController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }
        
        public override async Task<CreatePetListsReply> CreatePetLists(CreatePetListsRequest request, ServerCallContext context)
        {
            if(!Guid.TryParse(request.UserId, out Guid userId))
            {
                throw new InvalidUserIdException(request.UserId);
            }
            
            await _commandDispatcher.SendAsync(new CreateUserLikeList(userId));
            
            return new CreatePetListsReply();
        }

        public override async Task<DeleteUserPetsReply> DeleteUserPets(DeleteUserPetsRequest request, ServerCallContext context)
        {
            if(!Guid.TryParse(request.UserId, out Guid userId))
            {
                throw new InvalidUserIdException(request.UserId);
            }
            
            await _commandDispatcher.SendAsync(new DeleteUserPetLists(userId));
        
            return new DeleteUserPetsReply();        
        }
    }
}