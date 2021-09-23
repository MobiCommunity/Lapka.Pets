using System.Collections.Generic;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Pets.Application.Events
{
    public class LostPetPhotosRemoved : IEvent
    {
        public IEnumerable<string> PhotoPaths { get; }

        public LostPetPhotosRemoved(IEnumerable<string> photoPaths)
        { 
            PhotoPaths = photoPaths;
        }
    }
}