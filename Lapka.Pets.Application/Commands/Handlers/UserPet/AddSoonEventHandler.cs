using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class AddSoonEventHandler : ICommandHandler<AddSoonEvent>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<UserPet> _repository;

        public AddSoonEventHandler(IEventProcessor eventProcessor, IPetRepository<UserPet> repository)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
        }
        public async Task HandleAsync(AddSoonEvent command)
        {
            UserPet pet = await _repository.GetByIdAsync(command.PetId);
            if (!pet.UserId.Equals(command.UserId))
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.ToString());
            }
            
            pet.AddSoonEvent(command.SoonEvent);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}