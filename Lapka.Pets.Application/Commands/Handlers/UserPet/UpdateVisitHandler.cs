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
            UserPet pet = await _repository.GetByIdAsync(command.PetId);
            UserPetHelpers.ValidateUserAndPet(command.UserId, command.PetId, pet);


            Visit visitToUpdate = pet.LastVisits.FirstOrDefault(x => x.Id == command.UpdatedVisit.Id);
            if (visitToUpdate == null)
            {
                throw new VisitNotFoundException(command.UpdatedVisit.Id.ToString());
            }
            
            pet.UpdateLastVisit(visitToUpdate, command.UpdatedVisit);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}