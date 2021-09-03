using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateVisitHandler : ICommandHandler<UpdateVisit>
    {
        private readonly IUserPetService _petService;

        public UpdateVisitHandler(IUserPetService petService)
        {
            _petService = petService;
        }
        public async Task HandleAsync(UpdateVisit command)
        {
            UserPet pet = await _petService.GetAsync(command.PetId);
            _petService.ValidIfUserIsOwnerOfPet(pet, command.UserId);
            
            Visit visitToUpdate = GetVisitToUpdateFromPet(command.UpdatedVisit.Id, pet);
            pet.UpdateLastVisit(visitToUpdate, command.UpdatedVisit);

            await _petService.UpdateAsync(pet);
        }

        private static Visit GetVisitToUpdateFromPet(Guid visitId, UserPet pet)
        {
            Visit visitToUpdate = pet.LastVisits.FirstOrDefault(x => x.Id == visitId);
            if (visitToUpdate == null)
            {
                throw new VisitNotFoundException(visitId.ToString());
            }

            return visitToUpdate;
        }
    }
}