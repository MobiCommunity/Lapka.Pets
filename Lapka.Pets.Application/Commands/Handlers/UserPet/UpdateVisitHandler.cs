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
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserPetRepository _repository;

        public UpdateVisitHandler(IEventProcessor eventProcessor, IUserPetRepository repository)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
        }
        public async Task HandleAsync(UpdateVisit command)
        {
            UserPet pet = await _repository.GetByIdAsync(command.PetId);
            if (pet == null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }
            
            Visit visitToUpdate = GetVisitToUpdateFromPet(command.UpdatedVisit.Id, pet);
            pet.UpdateLastVisit(visitToUpdate, command.UpdatedVisit);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);        }

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