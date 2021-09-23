using Convey.CQRS.Events;
using System.Collections.Generic;
using System.Linq;
using Lapka.Pets.Application.Events;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Events.Abstract;
using Lapka.Pets.Core.Events.Concrete.Pets.Losts;
using Lapka.Pets.Core.Events.Concrete.Pets.Shelters;
using Lapka.Pets.Core.Events.Concrete.Pets.Users;

namespace Lapka.Pets.Infrastructure.Services
{
    public class DomainToIntegrationEventMapper : IDomainToIntegrationEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events) => events.Select(Map);

        public IEvent Map(IDomainEvent @event) => @event switch
        {
            LostPetPhotosDeleted lostPetPhotoDeleted => new LostPetPhotosRemoved(lostPetPhotoDeleted.DeletedPhotoPaths),
            ShelterPetPhotosDeleted shelterPetPhotoDeleted => new ShelterPetPhotosRemoved(shelterPetPhotoDeleted.DeletedPhotoPaths),
            UserPetPhotosDeleted userPetPhotoDeleted => new UserPetPhotosRemoved(userPetPhotoDeleted.DeletedPhotoPaths),
            LostPetDeleted lostPetDeleted => new LostPetRemoved(lostPetDeleted.Pet.Id.Value),
            ShelterPetDeleted shelterPetDeleted => new ShelterPetRemoved(shelterPetDeleted.Pet.Id.Value),
            UserPetDeleted userPetDeleted => new UserPetRemoved(userPetDeleted.Pet.Id.Value),
            _ => null
        };
    }
}