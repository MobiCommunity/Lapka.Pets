using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.Handlers.Helpers;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateVisitHandler : ICommandHandler<UpdateVisit>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<UserPet> _repository;

        public UpdateVisitHandler(IEventProcessor eventProcessor, IPetRepository<UserPet> repository)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
        }
        public async Task HandleAsync(UpdateVisit command)
        {
            UserPet pet = await UserPetHelpers.GetUserPetWithValidation(_repository, command.PetId, command.UserId);

            Visit visitToUpdate = GetVisitToUpdateFromPet(command.UpdatedVisit.Id, pet);
            pet.UpdateLastVisit(visitToUpdate, command.UpdatedVisit);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
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