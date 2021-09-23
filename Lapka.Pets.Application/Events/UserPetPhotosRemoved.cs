using System.Collections.Generic;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Pets.Application.Events
{
    public class UserPetPhotosRemoved : IEvent
    {
        public IEnumerable<string> PhotoPaths { get; }

        public UserPetPhotosRemoved(IEnumerable<string> photoPaths)
        { 
            PhotoPaths = photoPaths;
        }
    }
}